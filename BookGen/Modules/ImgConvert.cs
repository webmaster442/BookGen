//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Ui.ArgumentParser;
using BookGen.Utilities;
using SkiaSharp;

namespace BookGen.Modules
{
    internal class ImgConvert : ModuleWithState
    {
        protected ImgConvert(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "ImgConvert";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem(ModuleCommand,
                                            "-i",
                                            "--input",
                                            "-o",
                                            "--output",
                                            "-q",
                                            "--quality",
                                            "-w",
                                            "--width",
                                            "-h",
                                            "--height");
            }
        }


        public override bool Execute(string[] arguments)
        {
            var args = new ImgConvertArguments();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return false;
            }
        }
    }
}
