//-----------------------------------------------------------------------------
// (c) 2022-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Webmaster442.HttpServerFramework;

namespace BookGen.Infrastructure;

internal class ModuleApi : IModuleApi
{
    private readonly ILog _log;
    private readonly IServerLog _serverLog;
    private readonly IAppSetting _setting;
    private readonly ProgramInfo _programInfo;

    public ModuleApi(ILog log, IServerLog serverLog, IAppSetting setting, ProgramInfo programInfo)
    {
        _log = log;
        _serverLog = serverLog;
        _setting = setting;
        _programInfo = programInfo;
    }

    public Action<string, string[]>? OnExecuteModule { get; set; }
    public Func<IEnumerable<string>>? OnGetCommandNames { get; set; }
    public Func<string, string[]>? OnGetAutocompleteItems { get; set; }

    public void ExecuteModule(string module, string[] arguments)
    {
        OnExecuteModule?.Invoke(module, arguments);
    }

    public string[] GetAutoCompleteItems(string commandName)
    {
        return OnGetAutocompleteItems?.Invoke(commandName) ?? Array.Empty<string>();
    }

    public IEnumerable<string> GetCommandNames()
    {
        return OnGetCommandNames?.Invoke() ?? Enumerable.Empty<string>();
    }

    public void Wait(string exitString)
    {
        Console.WriteLine(exitString);
        if (!_programInfo.Gui && !_programInfo.NoWaitForExit)
        {
            Console.Read();
        }
    }

    public GeneratorRunner CreateRunner(bool verbose, string workDir)
    {
        _log.LogLevel = verbose ? LogLevel.Detail : LogLevel.Info;
        return new GeneratorRunner(_log, _serverLog, this, _setting, _programInfo, workDir);
    }
}