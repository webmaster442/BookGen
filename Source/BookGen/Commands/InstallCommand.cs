//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure.Terminal;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("install")]
internal class InstallCommand : AsyncCommand
{
    private readonly ILogger _logger;

    private class InstallOption
    {
        public required string DisplayText { get; init; }
        public required Task Action { get; init; }
    }

    public InstallCommand(ILogger logger)
    {
        _logger = logger;
    }

    public override SupportedOs SupportedOs => SupportedOs.Windows;

    public override async Task<int> ExecuteAsync(IReadOnlyList<string> context)
    {
        var menu = new InstallOption[]
        {
            new() {
                DisplayText = "Add install folder to PATH variable",
                Action = AddToPath(),
            },
            new() {
                DisplayText = "Install windows terminal profile",
                Action = InstallTerminalProfile()
            }
        };
        var selction = Terminal.SelectionMenu(menu, "Bookgen installer", "Select install options", f => f.DisplayText);

        foreach (var item in selction)
        {
            await item.Action;
        }

        return ExitCodes.Success;
    }

    private async Task InstallTerminalProfile()
    {
        var terminalInstall = new TerminalInstallCommand(_logger);
        await terminalInstall.ExecuteAsync(new TerminalInstallCommand.TerminalInstallArguments
        {
            CheckInstall = false,
            CheckTerminalInstall = false,
        }, Array.Empty<string>());
    }

    private Task AddToPath()
    {
        var currentFolder = Environment.CurrentDirectory;
        var path = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User) ?? string.Empty;
        var paths = path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        
        if (paths.Contains(currentFolder, StringComparer.OrdinalIgnoreCase))
        {
            _logger.LogInformation("Current folder is already in PATH variable");
            return Task.CompletedTask;
        }

        paths.Add(currentFolder);
        var newPath = string.Join(Path.PathSeparator, paths);
        Environment.SetEnvironmentVariable("PATH", newPath, EnvironmentVariableTarget.User);

        return Task.CompletedTask;
    }
}
