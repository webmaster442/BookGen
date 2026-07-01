//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli;
using BookGen.Cli.Annotations;

namespace BookGen;

public sealed class BuildArguments : BookGenArgumentBase
{
    [Switch("o", "output", true)]
    [Description("Required argument. Specifies the output directory name.")]
    public string OutputDirectory { get; set; } = string.Empty;

    [Switch("h", "host", false)]
    [Description("Optional argument. Specifies the host override for the book. If specified, then the book will be generated with the given host override.")]
    public string HostOverride { get; set; } = string.Empty;

    public override ValidationResult Validate(IValidationContext context)
    {
        ValidationResult originalResult = base.Validate(context);
        if (originalResult.IsOk
            && !string.IsNullOrEmpty(HostOverride)
            && !HostOverride.EndsWith('/'))
        {
            return ValidationResult.Error("Host override must end with a slash.");
        }
        return originalResult;
    }
}
