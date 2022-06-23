//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.DomainServices.Markdown;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.Interfaces;
using BookGen.Resources;

namespace BookGen.Modules
{
    internal class Md2HtmlModule : ModuleWithState
    {
        public Md2HtmlModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Md2HTML";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem(ModuleCommand,
                                            "-i",
                                            "--input",
                                            "-o",
                                            "--output",
                                            "-c",
                                            "--css",
                                            "-r",
                                            "--raw",
                                            "-ns",
                                            "--no-syntax");
            }
        }

        public override ModuleRunResult Execute(string[] arguments)
        {
            var args = new Md2HtmlArguments();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }


            string md = args.InputFile.ReadFile(CurrentState.Log);

            string pageTemplate = ResourceHandler.GetFile(KnownFile.TemplateSinglePageHtml);

            string cssForInline = "";
            if (FsPath.IsEmptyPath(args.Css))
            {
                cssForInline = ResourceHandler.GetFile(KnownFile.SinglePageCss);
            }
            else if (args.Css.IsExisting)
            {
                cssForInline = args.Css.ReadFile(CurrentState.Log);
            }

            using var pipeline = new BookGenPipeline(BookGenPipeline.Preview);
            pipeline.InjectPath(args.InputFile.GetDirectory());
            pipeline.SetSyntaxHighlightTo(!args.NoSyntax);

            string? mdcontent = pipeline.RenderMarkdown(md);

            string rendered;
            if (args.RawHtml)
            {
                rendered = mdcontent;
            }
            else
            {
                rendered = pageTemplate.Replace("<!--{css}-->", cssForInline);
                rendered = rendered.Replace("<!--{content}-->", mdcontent);
            }

            if (args.OutputFile.IsConsole)
            {
                WriteToStdout(rendered);
            }
            else
            {
                args.OutputFile.WriteFile(CurrentState.Log, rendered);
            }

            return ModuleRunResult.Succes;
        }

        private static void WriteToStdout(string rendered)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(rendered);
        }
    }
}
