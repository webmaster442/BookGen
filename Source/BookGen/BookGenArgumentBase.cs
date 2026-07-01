//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Commands;

namespace BookGen;

public class BookGenArgumentBase : ArgumentsBase, IVerbosablityToggle
{
    [Switch("v", "verbose", false)]
    [Description("Optional argument, turns on detailed logging. Usefull for locating issues")]
    public bool Verbose { get; set; }

    [Switch("d", "dir", true)]
    [Description("Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.")]
    public string Directory { get; set; }

    [Switch("co", "configoverlay", false)]
    [Description("Optional argument. Specifies a config overlay file. If specified, then the config overlay file will be loaded and merged with the default configuration.")]
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
