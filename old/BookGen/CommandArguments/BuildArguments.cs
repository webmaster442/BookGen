//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

public sealed class BuildArguments : BookGenArgumentBase
{
    [Switch("a", "action")]
    public BuildAction? Action { get; set; }

    [Switch("n", "nowait")]
    public bool NoWaitForExit { get; set; }

    public override ValidationResult Validate()
    {
        if (!Action.HasValue)
            return ValidationResult.Error("Action must be specified");

        return ValidationResult.Ok();
    }
}
