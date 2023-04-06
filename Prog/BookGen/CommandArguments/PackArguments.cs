//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

internal class PackArguments : BookGenArgumentBase
{
    [Switch("o", "output")]
    public string OutputFile { get; set; }

    public PackArguments()
    {
        OutputFile = string.Empty;
    }

    public override ValidationResult Validate()
    {
        if (string.IsNullOrEmpty(OutputFile))
            return ValidationResult.Error("Output file must be specified");

        return ValidationResult.Ok();
    }

    public override void ModifyAfterValidation()
    {
        var extension = Path.GetExtension(OutputFile);
        if (extension != ".zip")
            OutputFile = Path.ChangeExtension(OutputFile, ".zip");
    }
}
