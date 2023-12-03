//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.ConsoleUi;
using BookGen.Infrastructure;

namespace BookGen.Commands;

[CommandName("init")]
internal sealed class InitCommand : AsyncCommand<BookGenArgumentBase>, IDisposable
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
        _log.EnableVerboseLogingIfRequested(arguments);
        _log.CheckLockFileExistsAndExitWhenNeeded(arguments.Directory);

        _initMenu = new InitMenu(_log, new FsPath(arguments.Directory), _programInfo);
        await _initMenu.Run();
        return Constants.Succes;
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
