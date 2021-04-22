//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Ui.ArgumentParser;

namespace BookGen.Domain.ArgumentParsing
{
    internal class InputOutputArguments : ArgumentsBase
    {
        [Switch("i", "input", true)]
        public FsPath InputFile { get; set; }

        [Switch("o", "output", true)]
        public FsPath OutputFile { get; set; }

        public InputOutputArguments()
        {
            InputFile = FsPath.Empty;
            OutputFile = FsPath.Empty;
        }

        public override bool Validate()
        {
            return InputFile.IsExisting;
        }
    }
}
