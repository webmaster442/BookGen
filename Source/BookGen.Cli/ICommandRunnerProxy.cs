//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


namespace BookGen.Cli;

public interface ICommandRunnerProxy
{
    IEnumerable<string> CommandNames { get; }

    string[] GetAutoCompleteItems(string commandName);
    Task<int> RunCommand(string commandName, string[] argsToParse);
}