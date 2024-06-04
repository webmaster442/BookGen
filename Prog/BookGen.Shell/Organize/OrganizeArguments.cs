//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;

namespace BookGen.Shell.Organize;
internal class OrganizeArguments : ArgumentsBase
{
    [Argument(0, IsOptional = true)]
    public string Folder { get; set; }

    [Switch("s", "simulate")]
    public bool Simulate { get; set; }

    public OrganizeArguments()
    {
        Folder = Environment.CurrentDirectory;
    }

    public override ValidationResult Validate()
    {
        return string.IsNullOrEmpty(Folder) || !Directory.Exists(Folder)
            ? ValidationResult.Error($"Folder doesn't exist: {Folder}")
            : ValidationResult.Ok();
    }
}
