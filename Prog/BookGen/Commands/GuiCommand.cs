//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.ConsoleUi;
using BookGen.Infrastructure;

namespace BookGen.Commands;

[CommandName("gui")]
internal class GuiCommand : AsyncCommand<BookGenArgumentBase>, IDisposable
{
    private readonly ILog _log;
    private readonly IModuleApi _api;
    private readonly IHelpProvider _helpProvider;
    private readonly ProgramInfo _programInfo;

    private MainMenu? _mainMenu;
    private GeneratorRunner? _runner;

    public GuiCommand(ILog log, IModuleApi api, IHelpProvider helpProvider, ProgramInfo programInfo)
    {
        _log = log;
        _api = api;
        _helpProvider = helpProvider;
        _programInfo = programInfo;
    }

    public void Dispose()
    {
        if (_mainMenu != null)
        {
            _mainMenu.Dispose();
            _mainMenu = null;
        }
    }

    public override async Task<int> Execute(BookGenArgumentBase arguments, string[] context)
    {
        _programInfo.Gui = true;
        _runner = _api.CreateRunner(arguments.Verbose, arguments.Directory);

        _mainMenu = new MainMenu(_runner, _api, _helpProvider);

        _log.CheckLockFileExistsAndExitWhenNeeded(arguments.Directory);

        if (_mainMenu != null)
        {
            await _mainMenu.Run();
            return Constants.Succes;
        }

        return Constants.GeneralError;
    }
}
