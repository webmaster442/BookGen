﻿//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;
using BookGen.RenderEngine.Internals;

using Microsoft.Extensions.Logging;

namespace BookGen.RenderEngine.Functions;

internal sealed class Php : Function, IInjectable
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
            Name = "Php",
            Description = "Run PHP script and insert it's output",
            ArgumentInfos = new Internals.ArgumentInfo[]
            {
                new()
                {
                    Name = "file",
                    Description = "PHP script to execute",
                    Optional = false,
                }
            }
        };
    }

    public override string Execute(FunctionArguments arguments)
    {
        string? file = arguments.GetArgumentOrThrow<string>("file");
        _log.LogInformation("Trying to execute PHP CGI script: {file} ...", file);
        return _scriptProcess.ExecuteScriptProcess("php-cgi", _appSetting.PhpPath, file, _appSetting.PhpTimeout);
    }
}