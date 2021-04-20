//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Markdown;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Resources;
using BookGen.Ui.ArgumentParser;
using System;

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

        public override bool Execute(string[] arguments)
        {
            Md2HtmlParameters args = new Md2HtmlParameters();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return false;
            }


            string md = args.InputFile.ReadFile(CurrentState.Log);

            string pageTemplate = ResourceHandler.GetFile(KnownFile.TemplateSinglePageHtml);

            string cssForInline = "";
            if (args.Css.IsExisting)
            {
                cssForInline = args.Css.ReadFile(CurrentState.Log);
            }

            using var pipeline = new BookGenPipeline(BookGenPipeline.Preview);
            pipeline.InjectPath(args.InputFile.GetDirectory());
            pipeline.SetSyntaxHighlightTo(!args.NoSyntax);

            var mdcontent = pipeline.RenderMarkdown(md);

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

            if (args.OutputFile == new FsPath("con"))
            {
                Console.WriteLine(rendered);
            }
            else
            {
                args.OutputFile.WriteFile(CurrentState.Log, rendered);
            }

            return true;
        }
    }
}
