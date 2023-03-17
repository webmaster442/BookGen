using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.CommandArguments;
using BookGen.DomainServices.Markdown;
using BookGen.Interfaces;
using BookGen.Resources;

namespace BookGen.Commands
{
    [CommandName("md2html")]
    internal sealed class Md2HtmlCommand : Command<Md2HtmlArguments>
    {
        private readonly ILog _log;

        public Md2HtmlCommand(ILog log)
        {
            _log = log;
        }

        public override int Execute(Md2HtmlArguments arguments, string[] context)
        {
            string md = arguments.InputFile.ReadFile(_log);

            string pageTemplate = ResourceHandler.GetFile(KnownFile.TemplateSinglePageHtml);

            string cssForInline = "";
            if (FsPath.IsEmptyPath(arguments.Css))
            {
                cssForInline = ResourceHandler.GetFile(KnownFile.SinglePageCss);
            }
            else if (arguments.Css.IsExisting)
            {
                cssForInline = arguments.Css.ReadFile(_log);
            }

            using var pipeline = new BookGenPipeline(BookGenPipeline.Preview);
            pipeline.InjectPath(arguments.InputFile.GetDirectory());
            pipeline.SetSyntaxHighlightTo(!arguments.NoSyntax);

            string? mdcontent = pipeline.RenderMarkdown(md);

            string rendered;
            if (arguments.RawHtml)
            {
                rendered = mdcontent;
            }
            else
            {
                rendered = pageTemplate.Replace("<!--{css}-->", cssForInline);
                rendered = rendered.Replace("<!--{content}-->", mdcontent);
            }

            if (arguments.OutputFile.IsConsole)
            {
                WriteToStdout(rendered);
            }
            else
            {
                arguments.OutputFile.WriteFile(_log, rendered);
            }

            return Constants.Succes;
        }

        private static void WriteToStdout(string rendered)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(rendered);
        }
    }
}
