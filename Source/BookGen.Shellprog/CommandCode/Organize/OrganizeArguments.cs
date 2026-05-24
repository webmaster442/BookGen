//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli;
using BookGen.Cli.Annotations;

namespace BookGen.Shellprog.CommandCode.Organize;

internal class OrganizeArguments : ArgumentsBase
{
    [Argument(0, IsOptional = true)]
    [Description("Folder to organize")]
    public string Folder { get; set; }

    [Switch("s", "simulate")]
    [Description("Simulate the organize process without making any changes")]
    public bool Simulate { get; set; }

    public OrganizeArguments()
    {
        Folder = Environment.CurrentDirectory;
    }

    public override ValidationResult Validate(IValidationContext context)
    {
        return string.IsNullOrEmpty(Folder) || !context.FileSystem.DirectoryExists(Folder)
            ? ValidationResult.Error($"Folder doesn't exist: {Folder}")
            : ValidationResult.Ok();
    }
}
