//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Markdown;
using BookGen.Domain;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Resources;
using BookGen.Ui.ArgumentParser;
using BookGen.Utilities;
using System;

namespace BookGen.Modules
{
    internal class Md2HtmlModule : StateModuleBase
    {
        public Md2HtmlModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Md2HTML";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem("Md2HTML",
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

            var log = new ConsoleLog(LogLevel.Info);

            string md = args.InputFile.ReadFile(log);

            string pageTemplate = ResourceHandler.GetFile(KnownFile.TemplateSinglePageHtml);

            string cssForInline = "";
            if (args.Css.IsExisting)
            {
                cssForInline = args.Css.ReadFile(log);
            }

            var mdcontent = MarkdownRenderers.Markdown2Preview(md,
                                                               args.InputFile.GetDirectory(),
                                                               args.NoSyntax);
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
                args.OutputFile.WriteFile(log, rendered);
            }

            return true;
        }

        public override string GetHelp()
        {
            return HelpUtils.GetHelpForModule(nameof(Md2HtmlModule));
        }
    }
}
