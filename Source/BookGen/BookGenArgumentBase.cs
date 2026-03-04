//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Commands;

namespace BookGen;

public class BookGenArgumentBase : ArgumentsBase, IVerbosablityToggle
{
    [Switch("v", "verbose")]
    public bool Verbose { get; set; }

    [Switch("d", "dir")]
    public string Directory { get; set; }

    [Switch("co", "configoverlay")]
    public string ConfigOverlay { get; set; } = string.Empty;

    public BookGenArgumentBase()
    {
        Directory = Environment.CurrentDirectory;
    }

    override public ValidationResult Validate(IValidationContext context)
    {
        if (!context.FileSystem.DirectoryExists(Directory))
        {
            return ValidationResult.Error($"Directory '{Directory}' does not exist.");
        }

        if (!string.IsNullOrEmpty(ConfigOverlay)
            && !context.FileSystem.FileExists(ConfigOverlay))
        {
            return ValidationResult.Error($"Config overlay file '{ConfigOverlay}' does not exist.");
        }

        return ValidationResult.Ok();
    }
}
