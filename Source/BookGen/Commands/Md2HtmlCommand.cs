//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

using Bookgen.Lib;
using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.Markdown;
using Bookgen.Lib.Markdown.RenderInterop;
using Bookgen.Lib.Templates;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("md2html")]
internal sealed class Md2HtmlCommand : Command<Md2HtmlCommand.Md2HtmlArguments>
{
    internal sealed class Md2HtmlArguments : ArgumentsBase
    {
        [Switch("i", "input")]
        public string[] InputFiles { get; set; }

        [Switch("o", "output")]
        public string OutputFile { get; set; }

        [Switch("tf", "template")]
        public string Template { get; set; }

        [Switch("ns", "no-syntax")]
        public bool NoSyntax { get; set; }

        [Switch("ne", "no-embed")]
        public bool NoEmbed { get; set; }

        [Switch("r", "raw")]
        public bool RawHtml { get; set; }

        [Switch("s", "svg")]
        public bool SvgPassthrough { get; set; }

        [Switch("t", "title")]
        public string Title { get; set; }


        public Md2HtmlArguments()
        {
            Template = string.Empty;
            Title = "Markdown document";
            InputFiles = [];
            OutputFile = string.Empty;
        }

        public override ValidationResult Validate(IValidationContext context)
        {
            ValidationResult result = new();

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
    private const string TitleTag = "{{Title}}";
    private const string ContentTag = "{{Content}}";

    private readonly TemplateEngine _templateEngine;

    public Md2HtmlCommand(ILogger log, IFileSystemFactory fileSystemFactory, IAssetSource assetSource)
    {
        _log = log;
        _fileSystemFactory = fileSystemFactory;
        _fileSystem = fileSystemFactory.CreateWritableFileSystem();
        _assetSource = assetSource;
        _templateEngine = new TemplateEngine(log, assetSource);
    }

    public override int Execute(Md2HtmlArguments arguments, IReadOnlyList<string> context)
    {
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
            RenderInterop = new RenderInterop(_assetSource, imgConfig)
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
        Spectre.Console.AnsiConsole.WriteLine(rendered);
    }
}
