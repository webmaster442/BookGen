//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli;

public sealed class CommandRunnerProxy
{
    private IEnumerable<string>? _commandNames;
    private Func<string, string[]>? _autoComplete;

    public void ConfigureWith(CommandRunner runner)
    {
        _commandNames = runner.CommandNames;
        _autoComplete = runner.GetAutoCompleteItems;
    }

    public IEnumerable<string> CommandNames => (IEnumerable<string>?)_commandNames
        ?? throw new InvalidOperationException("Provider hasn't been setup correctly");

    public string[] GetAutoCompleteItems(string commandName)
        => _autoComplete?.Invoke(commandName)
        ?? throw new InvalidOperationException("Provider hasn't been setup correctly");
}
