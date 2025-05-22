//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli;

public sealed class CommandRunnerProxy
{
    private IEnumerable<string>? _commandNames;
    private Func<string, string[]>? _autoComplete;
    private Func<string, string[], Task<int>>? _runCommand;

    public void ConfigureWith(CommandRunner runner)
    {
        _commandNames = runner.CommandNames;
        _autoComplete = runner.GetAutoCompleteItems;
        _runCommand = runner.RunCommand;
    }

    public IEnumerable<string> CommandNames => (IEnumerable<string>?)_commandNames
        ?? throw new InvalidOperationException("Provider hasn't been setup correctly");

    public string[] GetAutoCompleteItems(string commandName)
        => _autoComplete?.Invoke(commandName)
        ?? throw new InvalidOperationException("Provider hasn't been setup correctly");

    public async Task<int> RunCommand(string commandName, string[] argsToParse)
    {
        if (_runCommand == null)
            throw new InvalidOperationException("Provider hasn't been setup correctly");

        return await _runCommand.Invoke(commandName, argsToParse);
    }
}
