//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Shell.Shared;

using Microsoft.Extensions.Logging;

using Webmaster442.WindowsTerminal;


namespace BookGen.Commands;

[CommandName("install")]
[Description("Windows only command that installs BookGen to the system PATH & optionally to the windows terminal.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
internal sealed class InstallCommand : AsyncCommand<InstallCommand.Arguments>
{
    internal sealed class Arguments : ArgumentsBase
    {
        [Switch("ctp", "check-terminal-profile", Required = false)]
        [Description("When specified checks, if terminal profile installed or not. If exit code is 0, profile is installed.")]
        public bool CheckTerminalProfileInstall { get; set; }

        [Switch("ct", "check-terminal-install", Required = false)]
        [Description("When specified checks, if windows terminal is installed or not. If exit code is 0, terminal is installed.")]
        public bool CheckTerminalInstall { get; set; }
    }

    private sealed class InstallOption
    {
        public required string DisplayText { get; init; }
        public required Task Action { get; init; }
    }

    private readonly ILogger _logger;

    public InstallCommand(ILogger logger)
    {
        _logger = logger;
    }

    public override SupportedOs SupportedOs
        => SupportedOs.Windows;

    private async Task InstallTerminalProfile()
    {
        var result = await TerminalProfileInstaller.TryInstallAsync();
        if (result == null)
        {
            _logger.LogWarning("Windows terminal is not installed, can't proceed");
            Environment.Exit(ExitCodes.GeneralError);
        }
        else if (result == false)
        {
            _logger.LogCritical("Terminal profile install failed");
            Environment.Exit(ExitCodes.GeneralError);
        }
        _logger.LogInformation("Successfully installed windows terminal profile");
    }

    private Task AddToPath()
    {
        var currentFolder = Environment.CurrentDirectory;
        var path = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User) ?? string.Empty;
        List<string> paths = path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

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

    public override async Task<int> ExecuteAsync(Arguments arguments, IReadOnlyList<string> context)
    {
        if (arguments.CheckTerminalInstall)
        {
            InstallResult installReult = InstallDetector.GetInstallResult();
            return installReult.IsWindowsTerminalInstalled ? ExitCodes.Success : ExitCodes.GeneralError;
        }

        if (arguments.CheckTerminalProfileInstall)
        {
            bool installed = TerminalProfileInstaller.IsInstalled();
            return installed ? ExitCodes.Success : ExitCodes.GeneralError;
        }

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
        List<InstallOption> selction = Infrastructure.Terminal.Terminal.SelectionMenu(menu, "Bookgen installer", "Select install options", f => f.DisplayText);

        foreach (InstallOption item in selction)
        {
            await item.Action;
        }

        return ExitCodes.Success;
    }
}
