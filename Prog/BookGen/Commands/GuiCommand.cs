using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.CommandArguments;
using BookGen.ConsoleUi;
using BookGen.Framework;
using BookGen.Infrastructure;

namespace BookGen.Commands
{
    [CommandName("gui")]
    internal class GuiCommand : AsyncCommand<BookGenArgumentBase>, IDisposable
    {
        private readonly ILog _log;
        private readonly IModuleApi _api;
        private readonly ProgramInfo _programInfo;

        private MainMenu? _mainMenu;
        private GeneratorRunner? _runner;
        private FolderLock? _folderLock;

        public GuiCommand(ILog log, IModuleApi api, ProgramInfo programInfo) 
        {
            _log = log;
            _api = api;
            _programInfo = programInfo;
        }

        public void Dispose()
        {
            if (_mainMenu != null)
            {
                _mainMenu.Dispose();
                _mainMenu = null;
            }
            if (_folderLock != null)
            {
                _folderLock.Dispose();
                _folderLock = null;
            }
        }

        public override async Task<int> Execute(BookGenArgumentBase arguments, string[] context)
        {
            _programInfo.Gui = true;
            _runner = _api.CreateRunner(arguments.Verbose, arguments.Directory);

            _mainMenu = new MainMenu(_runner, _api);

            _log.CheckLockFileExistsAndExitWhenNeeded(arguments.Directory);

            _folderLock = new FolderLock(arguments.Directory);

            if (_mainMenu != null)
            {
                await _mainMenu.Run();
                return Constants.Succes;
            }

            return Constants.GeneralError;
        }
    }
}
