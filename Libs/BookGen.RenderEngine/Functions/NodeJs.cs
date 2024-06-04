//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Interfaces;
using BookGen.RenderEngine.Internals;

namespace BookGen.RenderEngine.Functions;
internal sealed class NodeJs : Function, IInjectable
{
    private IAppSetting _appSetting = null!;
    private ILog _log = null!;
    private ScriptProcess _scriptProcess = null!;

    public void Inject(FunctionServices functionServices)
    {
        _appSetting = functionServices.AppSetting;
        _log = functionServices.Log;
        _scriptProcess = new ScriptProcess(functionServices.Log);
    }

    protected override FunctionInfo GetInformation()
    {
        return new FunctionInfo
        {
            Name = "NodeJs",
            Description = "Execute a NodeJs script and insert it's output",
            ArgumentInfos = new Internals.ArgumentInfo[]
            {
                new()
                {
                    Name = "file",
                    Description = "NodeJs script to execute",
                    Optional = false,
                }
            }
        };
    }

    public override string Execute(FunctionArguments arguments)
    {
        string? file = arguments.GetArgumentOrThrow<string>("file");
        _log.Info("Trying to execute NodeJs script: {0} ...", file);
        return _scriptProcess.ExecuteScriptProcess("node", _appSetting.NodeJsPath, file, _appSetting.NodeJsTimeout);
    }
}
