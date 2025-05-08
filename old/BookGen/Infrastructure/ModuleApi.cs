//-----------------------------------------------------------------------------
// (c) 2022-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli.Mediator;

namespace BookGen.Infrastructure;

internal sealed class ModuleApi : IModuleApi
{
    private readonly ILogger _log;
    private readonly IMediator _mediator;
    private readonly IAppSetting _setting;
    private readonly ProgramInfo _programInfo;
    private readonly TimeProvider _timeProvider;

    public ModuleApi(ILogger log,
                     IMediator mediator,
                     IAppSetting setting,
                     ProgramInfo programInfo,
                     TimeProvider timeProvider)
    {
        _log = log;
        _mediator = mediator;
        _setting = setting;
        _programInfo = programInfo;
        _timeProvider = timeProvider;
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
        return OnGetAutocompleteItems?.Invoke(commandName) ?? [];
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
        _programInfo.EableVerboseLogging(verbose);
        return new GeneratorRunner(_log, this, _setting, _programInfo, _timeProvider, _mediator, workDir);
    }
}