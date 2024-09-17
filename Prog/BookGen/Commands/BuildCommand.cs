//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.Framework;
using BookGen.Infrastructure;

namespace BookGen.Commands;

[CommandName("build")]
internal class BuildCommand : AsyncCommand<BuildArguments>
{
    private readonly ILogger _log;
    private readonly IMutexFolderLock _folderLock;
    private readonly IModuleApi _moduleApi;

    public BuildCommand(ILogger log, IMutexFolderLock folderLock,  IModuleApi moduleApi)
    {
        _log = log;
        _folderLock = folderLock;
        _moduleApi = moduleApi;
    }

    public override async Task<int> Execute(BuildArguments arguments, string[] context)
    {
        _folderLock.CheckLockFileExistsAndExitWhenNeeded(_log, arguments.Directory);

        GeneratorRunner? runner = _moduleApi.CreateRunner(arguments.Verbose, arguments.Directory);
        runner.NoWait = arguments.NoWaitForExit;

        switch (arguments.Action)
        {
            case BuildAction.BuildWeb:
                await runner.InitializeAndExecute(x => x.DoBuild());
                break;
            case BuildAction.Clean:
                await runner.InitializeAndExecute(x => x.DoClean());
                break;
            case BuildAction.Test:
                await runner.InitializeAndExecute(x => x.DoTest());
                break;
            case BuildAction.BuildPrint:
                await runner.InitializeAndExecute(x => x.DoPrint());
                break;
            case BuildAction.BuildWordpress:
                await runner.InitializeAndExecute(x => x.DoWordpress());
                break;
            case BuildAction.BuildEpub:
                await runner.InitializeAndExecute(x => x.DoEpub());
                break;
            case BuildAction.BuildPostprocess:
                await runner.InitializeAndExecute(x => x.DoPostProcess());
                break;
            case BuildAction.ValidateConfig:
                runner.Initialize();
                break;
        }

        return Constants.Succes;
    }
}
