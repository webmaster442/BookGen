//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


using BookGen.Interfaces;
using BookGen.RenderEngine.Internals;

using Microsoft.Extensions.Logging;

namespace BookGen.RenderEngine.Functions;

internal sealed class Python : Function, IInjectable
{
    private IAppSetting _appSetting = null!;
    private ILogger _log = null!;
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
            Name = "Python",
            Description = "Execute a Python script and insert it's output",
            ArgumentInfos = new Internals.ArgumentInfo[]
            {
                new()
                {
                    Name = "file",
                    Description = "Python script to execute",
                    Optional = false,
                }
            }
        };
    }

    public override string Execute(FunctionArguments arguments)
    {
        string? file = arguments.GetArgumentOrThrow<string>("file");
        _log.LogInformation("Trying to execute Python script: {file} ...", file);
        return _scriptProcess.ExecuteScriptProcess("python", _appSetting.PythonPath, file, _appSetting.PythonTimeout);
    }
}