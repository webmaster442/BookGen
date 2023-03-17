//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Interfaces;

namespace BookGen.CommandArguments
{
    public class InputOutputArguments : ArgumentsBase
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

        public override ValidationResult Validate()
        {
            if (!InputFile.IsExisting)
                return ValidationResult.Error("Input file doesn't exist");

            return ValidationResult.Ok();
        }
    }
}
