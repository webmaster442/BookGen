//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

internal sealed class JsonArgsArguments : BookGenArgumentBase
{
    [Switch("c", "command")]
    public string CommandName { get; set; }

    public JsonArgsArguments()
    {
        CommandName = string.Empty;
    }

    public override ValidationResult Validate()
    {
        if (string.IsNullOrWhiteSpace(CommandName))
        {
            return ValidationResult.Error("CommandName is missing");
        }
        return ValidationResult.Ok();
    }
}
