//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.Domain.Configuration;
using BookGen.DomainServices.Markdown;
using BookGen.Framework;
using BookGen.Resources;

namespace BookGen.Commands;

[CommandName("md2html")]
internal sealed class Md2HtmlCommand : Command<Md2HtmlArguments>, IDisposable
{
    private readonly ILog _log;

    private const string TitleTag = "<!--{title}-->";
    private const string CssTag = "<!--{css}-->";
    private const string ContentTag = "<!--{content}-->";

    private ShortCodeLoader? _loader;
    private readonly ShortCodeParser _parser;

    public Md2HtmlCommand(ILog log, IAppSetting appSetting, TimeProvider timeProvider)
    {
        _log = log;
        _loader = new ShortCodeLoader(_log,
                                      new RuntimeSettings(new EmptyTagUtils()),
                                      appSetting,
                                      timeProvider);
        _loader.LoadAll();
        _parser = new ShortCodeParser(_loader.Imports,
                                      new Translations(),
                                      _log);
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
            rendered = pageTemplate.Replace(TitleTag, arguments.Title);
            rendered = rendered.Replace(CssTag, cssForInline);
            rendered = rendered.Replace(ContentTag, mdcontent);
            rendered = _parser.Parse(rendered);
        }

        if (arguments.OutputFile.IsConsole)
            WriteToStdout(rendered);
        else
            arguments.OutputFile.WriteFile(_log, rendered);

        return Constants.Succes;
    }

    private string ReadInputFiles(FsPath[] inputFiles)
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
            _log.Critical("Template doesn't contain tag: {0}", TitleTag);
            returnValue = false;
        }

        if (!pageTemplate.Contains(CssTag))
        {
            _log.Critical("Template doesn't contain tag: {0}", CssTag);
            returnValue = false;
        }

        if (!pageTemplate.Contains(ContentTag))
        {
            _log.Critical("Template doesn't contain tag: {0}", ContentTag);
            returnValue = false;
        }

        return returnValue;
    }

    private static void WriteToStdout(string rendered)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine(rendered);
    }

    public void Dispose()
    {
        if (_loader != null)
        {
            _loader.Dispose();
            _loader = null;
        }
    }
}
