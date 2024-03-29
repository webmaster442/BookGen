﻿//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;

namespace BookGen.Shell.Cdg;

internal sealed class CdgArguments : ArgumentsBase
{
    [Switch("h", "hidden")]
    public bool ShowHidden { get; set; }

    [Argument(0, IsOptional = true)]
    public string Folder { get; set; }

    public CdgArguments()
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
