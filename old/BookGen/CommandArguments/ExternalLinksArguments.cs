//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

internal sealed class ExternalLinksArguments : BookGenArgumentBase
{
    [Switch("o", "output")]
    public FsPath OutputFile { get; set; }

    public ExternalLinksArguments()
    {
        OutputFile = FsPath.Empty;
    }

    public override ValidationResult Validate()
    {
        if (FsPath.IsEmptyPath(OutputFile))
            return ValidationResult.Error("Output file has to be specified");

        return ValidationResult.Ok();
    }
}
