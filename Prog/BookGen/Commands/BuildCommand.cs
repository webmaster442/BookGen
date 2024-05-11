//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.Framework;
using BookGen.Infrastructure;

namespace BookGen.Commands;

[CommandName("build")]
internal class BuildCommand : Command<BuildArguments>
{
    private readonly ILog _log;
    private readonly IMutexFolderLock _folderLock;
    private readonly IModuleApi _moduleApi;

    public BuildCommand(ILog log, IMutexFolderLock folderLock,  IModuleApi moduleApi)
    {
        _log = log;
        _folderLock = folderLock;
        _moduleApi = moduleApi;
    }

    public override int Execute(BuildArguments arguments, string[] context)
    {
        _folderLock.CheckLockFileExistsAndExitWhenNeeded(_log, arguments.Directory);

        GeneratorRunner? runner = _moduleApi.CreateRunner(arguments.Verbose, arguments.Directory);
        runner.NoWait = arguments.NoWaitForExit;

        switch (arguments.Action)
        {
            case BuildAction.BuildWeb:
                runner.InitializeAndExecute(x => x.DoBuild());
                break;
            case BuildAction.Clean:
                runner.InitializeAndExecute(x => x.DoClean());
                break;
            case BuildAction.Test:
                runner.InitializeAndExecute(x => x.DoTest());
                break;
            case BuildAction.BuildPrint:
                runner.InitializeAndExecute(x => x.DoPrint());
                break;
            case BuildAction.BuildWordpress:
                runner.InitializeAndExecute(x => x.DoWordpress());
                break;
            case BuildAction.BuildEpub:
                runner.InitializeAndExecute(x => x.DoEpub());
                break;
            case BuildAction.BuildPostprocess:
                runner.InitializeAndExecute(x => x.DoPostProcess());
                break;
            case BuildAction.ValidateConfig:
                runner.Initialize();
                break;
        }

        _log.Flush();
        return Constants.Succes;
    }
}
