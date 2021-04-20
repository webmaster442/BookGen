//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Compressor;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Ui.ArgumentParser;

namespace BookGen.Modules
{
    internal class CompressModule : ModuleWithState
    {
        protected CompressModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Compress";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem(ModuleCommand,
                                            "-i",
                                            "--input",
                                            "-o",
                                            "--output");
            }
        }

        public override bool Execute(string[] arguments)
        {
            InputOutputArguments args = new InputOutputArguments();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return false;
            }

            string html = args.InputFile.ReadFile(CurrentState.Log);

            var result = HtmlCompressor.CompressHtml(html);

            args.OutputFile.WriteFile(CurrentState.Log, result);

            return true;
        }
    }
}
