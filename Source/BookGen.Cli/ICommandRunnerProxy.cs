//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli;

public interface ICommandRunnerProxy
{
    IEnumerable<string> CommandNames { get; }

    string[] GetAutoCompleteItems(string commandName);
    Task<int> RunCommand(string commandName, IReadOnlyList<string> argsToParse);
}