//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.ConsoleUi;
using BookGen.Framework;
using BookGen.Infrastructure;

namespace BookGen.Commands;

[CommandName("gui")]
internal class GuiCommand : AsyncCommand<BookGenArgumentBase>, IDisposable
{
    private readonly ILogger _log;
    private readonly IModuleApi _api;
    private readonly IHelpProvider _helpProvider;
    private readonly IMutexFolderLock _folderLock;
    private readonly ProgramInfo _programInfo;

    private MainMenu? _mainMenu;
    private GeneratorRunner? _runner;

    public GuiCommand(ILogger log,
                      IModuleApi api,
                      IHelpProvider helpProvider,
                      IMutexFolderLock folderLock,
                      ProgramInfo programInfo)
    {
        _log = log;
        _api = api;
        _helpProvider = helpProvider;
        _folderLock = folderLock;
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

        _folderLock.CheckLockFileExistsAndExitWhenNeeded(_log, arguments.Directory);

        await _mainMenu.Run();
        return Constants.Succes;
    }
}
