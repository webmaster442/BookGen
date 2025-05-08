//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

internal sealed class NewArguments : ArgumentsBase
{
    [Switch("t", "template")]
    public string TemplateName { get; set; }

    [Switch("o", "output")]
    public string Output { get; set; }

    public bool TemplateSelected => !string.IsNullOrEmpty(TemplateName);

    public NewArguments()
    {
        TemplateName = string.Empty;
        Output = string.Empty;
    }

    public override ValidationResult Validate()
    {
        if (string.IsNullOrEmpty(TemplateName)
            && string.IsNullOrEmpty(Output))
        {
            return ValidationResult.Ok();
        }

        if (string.IsNullOrEmpty(Output))
            return ValidationResult.Error("Output file not specified");

        return ValidationResult.Ok();
    }
}
