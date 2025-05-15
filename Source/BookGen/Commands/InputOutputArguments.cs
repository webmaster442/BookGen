//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;

namespace BookGen.Commands;

public class InputOutputArguments : ArgumentsBase
{
    [Switch("i", "input")]
    public string InputFile { get; set; }

    [Switch("o", "output")]
    public string OutputFile { get; set; }

    public InputOutputArguments()
    {
        InputFile = string.Empty;
        OutputFile = string.Empty;
    }

    public override ValidationResult Validate(IValidationContext context)
    {
        ValidationResult result = new();

        if (!File.Exists(InputFile))
            result.AddIssue("Input file doesn't exist");

        if (string.IsNullOrEmpty(OutputFile))
            result.AddIssue("Output file/directory must be specified");

        return result;
    }
}
