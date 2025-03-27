//-----------------------------------------------------------------------------
// (c) 2022-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.Domain.Terminal;

using Webmaster442.WindowsTerminal;

namespace BookGen.Commands;

[CommandName("terminalinstall")]
internal sealed class TerminalInstallCommand : AsyncCommand<TerminalInstallArguments>
{
    private readonly ILogger _log;

    public TerminalInstallCommand(ILogger log)
    {
        _log = log;
    }

    public override async Task<int> Execute(TerminalInstallArguments arguments, string[] context)
    {

        if (arguments.CheckTerminalInstall)
        {
            var installReult = InstallDetector.GetInstallResult();
            return installReult.IsWindowsTerminalInstalled ? Constants.Succes : Constants.GeneralError;
        }

        if (arguments.CheckInstall)
        {
            bool installed = TerminalProfileInstaller.IsInstalled();
            return installed ? Constants.Succes : Constants.GeneralError;
        }

        var result = await TerminalProfileInstaller.TryInstallAsync();

        if (result == null)
        {
            _log.LogWarning("Windows terminal is not installed, can't proceed");
            return Constants.GeneralError;
        }
        else if (result == false)
        {
            _log.LogCritical("Terminal profile install failed");
            return Constants.GeneralError;
        }

        _log.LogInformation("Successfully installed windows terminal profile");
        return Constants.Succes;
    }
}
