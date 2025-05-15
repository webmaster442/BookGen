//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

using BookGen.Cli;
using BookGen.Cli.Annotations;

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

        [Switch("c", "css")]
        public string Css { get; set; }

        [Switch("tf", "template")]
        public string Template { get; set; }

        [Switch("ns", "no-syntax")]
        public bool NoSyntax { get; set; }

        [Switch("r", "raw")]
        public bool RawHtml { get; set; }

        [Switch("nc", "no-css")]
        public bool NoCss { get; set; }

        [Switch("s", "svg")]
        public bool SvgPassthrough { get; set; }

        [Switch("t", "title")]
        public string Title { get; set; }


        public Md2HtmlArguments()
        {
            Css = string.Empty;
            Template = string.Empty;
            Title = "Markdown document";
            InputFiles = [];
            OutputFile = string.Empty;
        }

        public override ValidationResult Validate(IValidationContext context)
        {
            ValidationResult result = new();

            if (!string.IsNullOrEmpty(Template))
            {
                if (!context.FileSystem.FileExists(Template))
                    result.AddIssue("Template file doesn't exist");
            }

            if (!string.IsNullOrEmpty(Css))
            {
                if (!context.FileSystem.FileExists(Css))
                    result.AddIssue("css file doesn't exist");
            }

            if (string.IsNullOrEmpty(OutputFile))
                result.AddIssue("Output file must be specified");

            if (!InputFiles.Any())
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

    private const string TitleTag = "{{title}}";
    private const string CssTag = "{{css}}";
    private const string ContentTag = "{{content}}";

    private readonly TemplateRenderer _renderer;

    public Md2HtmlCommand(ILogger log, TimeProvider timeProvider)
    {
        _log = log;


        _renderer = new TemplateRenderer(new FunctionServices
        {
            AppSetting = appSetting,
            Log = log,
            TimeProvider = timeProvider,
            RuntimeSettings = new RuntimeSettings()
            {
                SourceDirectory = FsPath.Empty,
                Configuration = new Config(),
                TocContents = new ToC(),
                MetataCache = new Dictionary<string, string>(),
                InlineImgCache = new ConcurrentDictionary<string, string>(),
                CurrentBuildConfig = new BuildConfig(),
                Tags = new EmptyTagUtils()
            }
        });
    }

    public override int Execute(Md2HtmlArguments arguments, string[] context)
    {
        string md = ReadInputFiles(arguments.InputFiles);

        string pageTemplate = string.Empty;

        if (arguments.Template == FsPath.Empty)
            pageTemplate = ResourceHandler.GetFile(KnownFile.TemplateSinglePageHtml);
        else
            pageTemplate = arguments.Template.ReadFile(_log);

        if (!ValidateTemplate(pageTemplate))
            return Constants.GeneralError;

        string cssForInline = "/*no inline css was specified*/";
        if (FsPath.IsEmptyPath(arguments.Css) && !arguments.NoCss)
            cssForInline = ResourceHandler.GetFile(KnownFile.SinglePageCss);
        else if (arguments.Css.IsExisting)
            cssForInline = arguments.Css.ReadFile(_log);

        using var pipeline = new BookGenPipeline(BookGenPipeline.Preview);
        pipeline.InjectPath(arguments.InputFiles[0].GetDirectory());
        pipeline.SetSyntaxHighlightTo(!arguments.NoSyntax);
        pipeline.SetSvgPasstroughTo(arguments.SvgPassthrough);

        string? mdcontent = pipeline.RenderMarkdown(md);

        string rendered;
        if (arguments.RawHtml)
        {
            rendered = mdcontent;
        }
        else
        {
            var parameters = new TemplateParameters
            {
                Title = arguments.Title,
                Content = mdcontent,
            };
            parameters.Add("css", cssForInline);

            rendered = _renderer.Render(pageTemplate, parameters);
        }

        if (arguments.OutputFile.IsConsole)
            WriteToStdout(rendered);
        else
            arguments.OutputFile.WriteFile(_log, rendered);

        return Constants.Succes;
    }

    private string ReadInputFiles(string inputFiles)
    {
        StringBuilder md = new(inputFiles.Length * 1024);
        foreach (var inputFile in inputFiles)
        {
            string content = inputFile.ReadFile(_log);
            md.Append(content);
            if (!content.EndsWith('\n'))
                md.Append(Environment.NewLine);
        }
        return md.ToString();
    }

    private bool ValidateTemplate(string pageTemplate)
    {
        bool returnValue = true;
        if (!pageTemplate.Contains(TitleTag))
        {
            _log.LogCritical("Template doesn't contain tag: {tag}", TitleTag);
            returnValue = false;
        }

        if (!pageTemplate.Contains(CssTag))
        {
            _log.LogCritical("Template doesn't contain tag: {tag}", CssTag);
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
        Console.WriteLine(rendered);
    }
}
