//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;

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
