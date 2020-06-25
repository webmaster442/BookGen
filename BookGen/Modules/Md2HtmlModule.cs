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

        private bool TryGetMd2HtmlParameters(ArgumentParser arguments, out Md2HtmlParameters md2HtmlParameters)
        {
            md2HtmlParameters = new Md2HtmlParameters(
                arguments.GetSwitchWithValue("-i", "--input"),
                arguments.GetSwitchWithValue("-o", "--output"),
                arguments.GetSwitchWithValue("-c", "--css"),
                arguments.GetSwitch("-ns", "--no-syntax"),
                arguments.GetSwitch("-r", "--raw"));

            if (md2HtmlParameters.Css != FsPath.Empty)
            {
                return 
                    md2HtmlParameters.Css.IsExisting
                    && md2HtmlParameters.InputFile.IsExisting;
            }

            return
                md2HtmlParameters.InputFile.IsExisting;
        }

        public override bool Execute(ArgumentParser tokenizedArguments)
        {
            if (!TryGetMd2HtmlParameters(tokenizedArguments, out Md2HtmlParameters parameters))
                return false;

            var log = new ConsoleLog(LogLevel.Info);

            string md = parameters.InputFile.ReadFile(log);

            string pageTemplate = ResourceHandler.GetFile(KnownFile.TemplateSinglePageHtml);

            string cssForInline = "";
            if (parameters.Css.IsExisting)
            {
                cssForInline = parameters.Css.ReadFile(log);
            }

            var mdcontent = MarkdownRenderers.Markdown2Preview(md, 
                                                               parameters.InputFile.GetDirectory(),
                                                               parameters.NoSyntax);


            string rendered;
            if (parameters.RawHtml)
            {
                rendered = mdcontent;
            }
            else
            {
                rendered = pageTemplate.Replace("<!--{css}-->", cssForInline);
                rendered = rendered.Replace("<!--{content}-->", mdcontent);
            }

            if (parameters.OutputFile == new FsPath("con"))
            {
                Console.WriteLine(rendered);
            }
            else
            {
                parameters.OutputFile.WriteFile(log, rendered);
            }

            return true;
        }

        public override string GetHelp()
        {
            return HelpUtils.GetHelpForModule(nameof(Md2HtmlModule));
        }
    }
}
