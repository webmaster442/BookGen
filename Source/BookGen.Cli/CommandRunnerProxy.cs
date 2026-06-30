//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli.OpenCli.Draft;

namespace BookGen.Cli;

public sealed class CommandRunnerProxy : ICommandRunnerProxy
{
    private Func<string, string[]>? _autoComplete;
    private Func<string, IReadOnlyList<string>, Task<int>>? _runCommand;
    private Func<Document>? _openCliGenerator;
    private IEnumerable<string>? _globalOptions;

    public void ConfigureWith(CommandRunner runner)
    {
        CommandNames = runner.CommandNames;
        _autoComplete = runner.GetAutoCompleteItems;
        _runCommand = runner.RunCommand;
        _openCliGenerator = runner.GenerateOpenCliDocs;
        _globalOptions = runner.GetGlobalOptions();
    }

    public IEnumerable<string> CommandNames
    {
        get => field ?? throw new InvalidOperationException("Provider hasn't been setup correctly");
        private set;
    }

    public IEnumerable<string> GlobalOptions
        => _globalOptions 
        ?? throw new InvalidOperationException("Provider hasn't been setup correctly");

    public string[] GetAutoCompleteItems(string commandName)
        => _autoComplete?.Invoke(commandName)
        ?? throw new InvalidOperationException("Provider hasn't been setup correctly");

    public async Task<int> RunCommand(string commandName, IReadOnlyList<string> argsToParse)
    {
        if (_runCommand == null)
            throw new InvalidOperationException("Provider hasn't been setup correctly");

        return await _runCommand.Invoke(commandName, argsToParse);
    }

    public Document GetOpenCliDocs()
        => _openCliGenerator?.Invoke()
        ?? throw new InvalidOperationException("Provider hasn't been setup correctly");
}
