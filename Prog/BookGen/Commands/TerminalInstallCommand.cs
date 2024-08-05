using BookGen.CommandArguments;
using BookGen.Domain.Terminal;

namespace BookGen.Commands;

[CommandName("terminalinstall")]
internal sealed class TerminalInstallCommand : Command<TerminalInstallArguments>
{
    private readonly ILog _log;

    public TerminalInstallCommand(ILog log)
    {
        _log = log;
    }

    public override int Execute(TerminalInstallArguments arguments, string[] context)
    {

        if (arguments.CheckTerminalInstall)
        {
            InstallStatus installStatus = InstallDetector.GetInstallStatus();
            return installStatus.IsWindowsTerminalInstalled ? Constants.Succes : Constants.GeneralError;
        }

        if (arguments.CheckInstall)
        {
            bool installed = Directory.Exists(TerminalProfileInstaller.TerminalFragmentPath);
            if (installed)
            {
                installed &= Directory.GetFiles(TerminalProfileInstaller.TerminalFragmentPath, "*.json").Length > 0;
            }
            return installed ? Constants.Succes : Constants.GeneralError;
        }

        var result = TerminalProfileInstaller.TryInstall();

        if (result == null)
        {
            _log.Warning("Windows terminal is not installed, can't proceed");
            return Constants.GeneralError;
        }
        else if (result == false)
        {
            _log.Critical("Terminal profile install failed");
            return Constants.GeneralError;
        }

        _log.Info("Successfully installed windows terminal profile");
        return Constants.Succes;
    }
}
