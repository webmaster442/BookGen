using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.CommandArguments;
using BookGen.ConsoleUi;
using BookGen.Framework;
using BookGen.Infrastructure;
using BookGen.Interfaces;

namespace BookGen.Commands
{
    [CommandName("init")]
    internal class InitCommand : AsyncCommand<BookGenArgumentBase>
    {
        private InitMenu? _initMenu;
        private readonly ILog _log;
        private readonly ProgramInfo _programInfo;

        public InitCommand(ILog log, ProgramInfo programInfo) 
        {
            _log = log;
            _programInfo = programInfo;
        }

        public override async Task<int> Execute(BookGenArgumentBase arguments, string[] context)
        {
            _log.LogLevel = arguments.Verbose ? Api.LogLevel.Detail : Api.LogLevel.Info;

            _log.CheckLockFileExistsAndExitWhenNeeded(arguments.Directory);
            using (var l = new FolderLock(arguments.Directory))
            {

                _initMenu = new InitMenu(_log, new FsPath(arguments.Directory), _programInfo);
                await _initMenu.Run();
                return Constants.Succes;

            }
        }

        public void Dispose()
        {
            if (_initMenu != null)
            {
                _initMenu.Dispose();
                _initMenu = null;
            }
        }
    }
}
