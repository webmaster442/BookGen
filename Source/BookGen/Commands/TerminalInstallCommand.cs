//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Shell.Shared;

using Microsoft.Extensions.Logging;

using Webmaster442.WindowsTerminal;

namespace BookGen.Commands;

[CommandName("terminalinstall")]
internal sealed class TerminalInstallCommand : AsyncCommand<TerminalInstallCommand.TerminalInstallArguments>
{
    internal sealed class TerminalInstallArguments : ArgumentsBase
    {
        [Switch("c", "checkinstall")]
        public bool CheckInstall { get; set; }

        [Switch("t", "checkterminalinstall")]
        public bool CheckTerminalInstall { get; set; }
    }

    private readonly ILogger _log;

    public TerminalInstallCommand(ILogger log)
    {
        _log = log;
    }

    public override SupportedOs SupportedOs => SupportedOs.Windows;

    public override async Task<int> ExecuteAsync(TerminalInstallArguments arguments, IReadOnlyList<string> context)
    {

        if (arguments.CheckTerminalInstall)
        {
            InstallResult installReult = InstallDetector.GetInstallResult();
            return installReult.IsWindowsTerminalInstalled ? ExitCodes.Success : ExitCodes.GeneralError;
        }

        if (arguments.CheckInstall)
        {
            bool installed = TerminalProfileInstaller.IsInstalled();
            return installed ? ExitCodes.Success : ExitCodes.GeneralError;
        }

        var result = await TerminalProfileInstaller.TryInstallAsync();

        if (result == null)
        {
            _log.LogWarning("Windows terminal is not installed, can't proceed");
            return ExitCodes.GeneralError;
        }
        else if (result == false)
        {
            _log.LogCritical("Terminal profile install failed");
            return ExitCodes.GeneralError;
        }

        _log.LogInformation("Successfully installed windows terminal profile");
        return ExitCodes.Success;
    }
}
