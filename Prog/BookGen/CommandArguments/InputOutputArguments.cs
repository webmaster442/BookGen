//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

public class InputOutputArguments : ArgumentsBase
{
    [Switch("i", "input")]
    public FsPath InputFile { get; set; }

    [Switch("o", "output")]
    public FsPath OutputFile { get; set; }

    public InputOutputArguments()
    {
        InputFile = FsPath.Empty;
        OutputFile = FsPath.Empty;
    }

    public override ValidationResult Validate()
    {
        ValidationResult result = new();
        if (!InputFile.IsExisting)
            result.AddIssue("Input file doesn't exist");

        if (!FsPath.IsEmptyPath(OutputFile))
            result.AddIssue("Output file must be specified");

        return result;
    }
}
