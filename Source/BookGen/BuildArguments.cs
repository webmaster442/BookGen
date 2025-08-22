using BookGen.Cli;
using BookGen.Cli.Annotations;

namespace BookGen;

public sealed class BuildArguments : BookGenArgumentBase
{
    [Switch("o", "output")]
    public string OutputDirectory { get; set; } = string.Empty;

    [Switch("h", "host")]
    public string HostOverride { get; set; } = string.Empty;

    public override ValidationResult Validate(IValidationContext context)
    {
        var originalResult = base.Validate(context);
        if (originalResult.IsOk 
            && !string.IsNullOrEmpty(HostOverride)
            && !HostOverride.EndsWith('/'))
        {
            return ValidationResult.Error("Host override must end with a slash.");
        }
        return originalResult;
    }
}
