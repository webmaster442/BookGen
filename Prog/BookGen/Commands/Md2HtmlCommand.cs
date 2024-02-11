//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.Domain.Epub;
using BookGen.DomainServices.Markdown;
using BookGen.Resources;

namespace BookGen.Commands;

[CommandName("md2html")]
internal sealed class Md2HtmlCommand : Command<Md2HtmlArguments>
{
    private readonly ILog _log;

    private const string TitleTag = "<!--{title}-->";
    private const string CssTag = "<!--{css}-->";
    private const string ContentTag = "<!--{content}-->";

    public Md2HtmlCommand(ILog log)
    {
        _log = log;
    }

    public override int Execute(Md2HtmlArguments arguments, string[] context)
    {
        string md = arguments.InputFile.ReadFile(_log);

        string pageTemplate = string.Empty;

        if (arguments.Template == FsPath.Empty)
            pageTemplate = ResourceHandler.GetFile(KnownFile.TemplateSinglePageHtml);
        else
            pageTemplate = arguments.Template.ReadFile(_log);

        if (!ValidateTemplate(pageTemplate))
            return Constants.GeneralError;

        string cssForInline = string.Empty;
        if (FsPath.IsEmptyPath(arguments.Css))
            cssForInline = ResourceHandler.GetFile(KnownFile.SinglePageCss);
        else if (arguments.Css.IsExisting)
            cssForInline = arguments.Css.ReadFile(_log);

        using var pipeline = new BookGenPipeline(BookGenPipeline.Preview);
        pipeline.InjectPath(arguments.InputFile.GetDirectory());
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
        }

        if (arguments.OutputFile.IsConsole)
            WriteToStdout(rendered);
        else
            arguments.OutputFile.WriteFile(_log, rendered);

        return Constants.Succes;
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
}
