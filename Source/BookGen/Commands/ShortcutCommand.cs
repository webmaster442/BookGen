//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Shell.Shared;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("shortcut")]
internal class ShortcutCommand : Command
{
    private readonly ILogger _logger;

    public ShortcutCommand(ILogger logger)
    {
        _logger = logger;
    }

    public override SupportedOs SupportedOs => SupportedOs.Windows;

    public override int Execute(IReadOnlyList<string> context)
    {
        string profileName = TerminalProfileInstaller.GetProfileTitle();

        string fileName = Path.Combine(AppContext.BaseDirectory, "bookgen.exe");

        string targetPath = Path.Combine(Environment.CurrentDirectory, "open bookgen shell here.lnk");

#pragma warning disable CA1416 // Validate platform compatibility
        LinkBuilder link = new();

        link.SetPath("wt.exe")
            .SetArguments($"-p \"{profileName}\" --startingDirectory .")
            .SetIconLocation(fileName, 0)
            .Save(targetPath);

#pragma warning restore CA1416 // Validate platform compatibility

        _logger.LogInformation("Shortcut created: {path}", targetPath);


        return ExitCodes.Success;
    }
}
