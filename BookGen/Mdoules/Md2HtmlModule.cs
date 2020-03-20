//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Markdown;
using BookGen.Domain.ArgumentParsing;
using BookGen.Template;
using BookGen.Utilities;

namespace BookGen.Mdoules
{
    internal class Md2HtmlModule : ModuleBase
    {
        public Md2HtmlModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Md2HTML";

        private bool GetMd2HtmlParameters(ArgumentParser arguments, out Md2HtmlParameters md2HtmlParameters)
        {
            md2HtmlParameters = new Md2HtmlParameters(
                arguments.GetSwitchWithValue("-i", "--input"),
                arguments.GetSwitchWithValue("-o", "--output"),
                arguments.GetSwitchWithValue("-c", "--css"));

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
            if (!GetMd2HtmlParameters(tokenizedArguments, out Md2HtmlParameters parameters))
                return false;

            var log = new ConsoleLog(LogLevel.Info);

            string md = parameters.InputFile.ReadFile(log);
            string rendered = BuiltInTemplates.Print.Replace("[content]", MarkdownRenderers.Markdown2Preview(md, parameters.InputFile.GetDirectory()));
            parameters.OutputFile.WriteFile(log, rendered);

            return true;
        }

        public override string GetHelp()
        {
            return HelpUtils.GetHelpForModule(nameof(Md2HtmlModule));
        }
    }
}
