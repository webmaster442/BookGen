//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Text;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Lib;
using BookGen.Lib.AppSettings;
using BookGen.Lib.Domain.IO.Configuration;
using BookGen.Lib.Rendering.Images;
using BookGen.Lib.Rendering.Markdown;
using BookGen.Lib.Rendering.Markdown.RenderInterop;
using BookGen.Lib.Rendering.Templates;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

using Spectre.Console;

namespace BookGen.Commands.Convert;

[CommandName("md2html")]
[Description("Renders a single markdown file to an HTML file.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
internal sealed class Md2HtmlCommand : Command<Md2HtmlCommand.Arguments>
{
    internal sealed class Arguments : ArgumentsBase
    {
        [Switch("i", "input", Required = true)]
        [Description("Input markdown file path. Multiple files can be set with multiple `-i` arguments")]
        public string[] InputFiles { get; set; }

        [Switch("o", "output", Required = true)]
        [Description("Output html file path. If file name is `-`, outputs to console.")]
        public string OutputFile { get; set; }

        [Switch("tf", "template", Required = false)]
        [Description("If not specified, default template is used. If custom file provided, then the file must contain the folloing tags: `<!--{Title}-->`, `<!--{Content}-->`")]
        public string Template { get; set; }

        [Switch("ns", "no-syntax", Required = false)]
        [Description("Disables syntax highlighting in the output HTML.")]
        public bool NoSyntax { get; set; }

        [Switch("ne", "no-embed", Required = false)]
        [Description("Disables embedding of assets in the output HTML.")]
        public bool NoEmbed { get; set; }

        [Switch("r", "raw", Required = false)]
        [Description("Disables full html generation, only outputs the html produced by the markdown formatting.")]
        public bool RawHtml { get; set; }

        [Switch("s", "svg", Required = false)]
        [Description("When enabled SVG files will be embedded in resulting html, instead of being rendered to webp.")]
        public bool SvgPassthrough { get; set; }

        [Switch("t", "title", Required = false)]
        [Description("Specifies the rendered HTML page title. Only has affect, when `-r` or `--raw` is not specified.")]
        public string Title { get; set; }

        [Switch("lt", "list-templates", Required = false)]
        [Description("When specified, lists all available built-in templates to used with the `-tf` or `--template` option and exits")]
        public bool ListTemplates { get; set; }

        public Arguments()
        {
            Template = string.Empty;
            Title = "Markdown document";
            InputFiles = [];
            OutputFile = string.Empty;
        }

        public override Cli.ValidationResult Validate(IValidationContext context)
        {
            Cli.ValidationResult result = new();

            if (!string.IsNullOrEmpty(Template)
                && context.IsValidTemplateFile(Template))
            {
                result.AddIssue($"Template file: {Template} doesn't exist");
            }

            if (string.IsNullOrEmpty(OutputFile))
                result.AddIssue("Output file must be specified");

            if (InputFiles.Length == 0)
                result.AddIssue("An Input file must be specified");

            foreach (var inputfile in InputFiles)
            {
                if (!context.FileSystem.FileExists(inputfile))
                    result.AddIssue($"Input file: {inputfile} doesn't exist");
            }

            if (string.IsNullOrWhiteSpace(Title))
                result.AddIssue("Title can't be only whitespaces or empty");

            return result;
        }
    }

    private readonly ILogger _log;
    private readonly IFileSystemFactory _fileSystemFactory;
    private readonly IWritableFileSystem _fileSystem;
    private readonly IAssetSource _assetSource;
    private readonly TemplateEngine _templateEngine;
    private readonly IProgramPathResolver _programPathResolver;

    private const string TitleTag = "{{Title}}";
    private const string ContentTag = "{{Content}}";

    public Md2HtmlCommand(ILogger log, IFileSystemFactory fileSystemFactory, IProgramPathResolver programPathResolver, IAssetSource assetSource)
    {
        _log = log;
        _fileSystemFactory = fileSystemFactory;
        _fileSystem = fileSystemFactory.CreateWritableFileSystem();
        _programPathResolver = programPathResolver;
        _assetSource = assetSource;
        _templateEngine = new TemplateEngine(log, assetSource);
    }

    public override int Execute(Arguments arguments, IReadOnlyList<string> context)
    {
        if (arguments.ListTemplates)
        {
            return ListTemplatesAndExit();
        }

        IEnumerable<string?> inputFolders = arguments.InputFiles.Select(i => Path.GetDirectoryName(i));

        IReadOnlyFileSystem inputFilesScope = _fileSystemFactory.CreateMultiReadScopeFileSystem(inputFolders!);

        (string md, DateTime lastmodified) = inputFilesScope.ReadInputFiles(arguments.InputFiles);

        string? pageTemplate = string.Empty;

        if (string.IsNullOrEmpty(arguments.Template))
            pageTemplate = _assetSource.GetAsset(BundledAssets.TemplateSinglePage);
        else
            pageTemplate = inputFilesScope.ReadAllText(arguments.Template);

        if (!ValidateTemplate(pageTemplate))
            return ExitCodes.GeneralError;

        var imgConfig = new ImageConfig
        {
            SvgRecode = arguments.SvgPassthrough ? SvgRecodeOption.Passtrough : SvgRecodeOption.AsWebp,
            ImageQualityOnResize = 90,
        };

        var imgService = new ImgService(inputFilesScope, _log, imgConfig);

        using var settings = new MarkdownRenderSettings(imgService)
        {
            HostUrl = string.Empty,
            DeleteFirstH1 = false,
            CssClasses = new CssClasses(),
            OffsetHeadingsBy = 0,
            AutoEmbedSupportedLinks = !arguments.NoEmbed,
            RenderInterop = new RenderInterop(_assetSource, _programPathResolver, imgConfig)
        };

        settings.RenderInterop.PreRenderCode = !arguments.NoSyntax;

        using var markdownConverter = new MarkdownConverter(settings);

        string? mdcontent = markdownConverter.RenderMarkdownToHtml(md);

        string rendered;
        if (arguments.RawHtml)
        {
            rendered = mdcontent;
        }
        else
        {
            var viewData = new ViewData
            {
                Host = string.Empty,
                Content = mdcontent,
                Title = arguments.Title,
                LastModified = lastmodified,
            };

            rendered = _templateEngine.Render(pageTemplate, viewData);
        }

        if (arguments.OutputFile == "-")
            WriteToStdout(rendered);
        else
            _fileSystem.WriteAllText(arguments.OutputFile, rendered);

        return ExitCodes.Success;
    }

    private static int ListTemplatesAndExit()
    {
        AnsiConsole.WriteLine("Available built-in templates:");
        foreach (var template in BundledAssets.Md2HtmlTemplates)
        {
            AnsiConsole.WriteLine($"- {template}");
        }
        return ExitCodes.Success;
    }

    private bool ValidateTemplate(string pageTemplate)
    {
        bool returnValue = true;
        if (!pageTemplate.Contains(TitleTag))
        {
            _log.LogCritical("Template doesn't contain tag: {tag}", TitleTag);
            returnValue = false;
        }

        if (!pageTemplate.Contains(ContentTag))
        {
            _log.LogCritical("Template doesn't contain tag: {tag}", ContentTag);
            returnValue = false;
        }

        return returnValue;
    }

    private static void WriteToStdout(string rendered)
    {
        Console.OutputEncoding = Encoding.UTF8;
        AnsiConsole.WriteLine(rendered);
    }
}
