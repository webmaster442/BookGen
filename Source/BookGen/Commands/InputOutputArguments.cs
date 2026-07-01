//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli;
using BookGen.Cli.Annotations;

namespace BookGen.Commands;

public class InputOutputArguments : ArgumentsBase
{
    [Switch("i", "input", true)]
    [Description("Required argument. Specifies the input file.")]
    public string InputFile { get; set; }

    [Switch("o", "output", true)]
    [Description("Required argument. Specifies the output file or path")]
    public string OutputFile { get; set; }

    public InputOutputArguments()
    {
        InputFile = string.Empty;
        OutputFile = string.Empty;
    }

    public override ValidationResult Validate(IValidationContext context)
    {
        ValidationResult result = new();

        if (!context.FileSystem.FileExists(InputFile))
            result.AddIssue("Input file doesn't exist");

        if (string.IsNullOrEmpty(OutputFile))
            result.AddIssue("Output file/directory must be specified");

        return result;
    }
}
