//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli.OpenCli.Draft;

namespace BookGen.Cli;

public interface ICommandRunnerProxy
{
    IEnumerable<string> CommandNames { get; }
    IEnumerable<string> GlobalOptions { get; }
    string[] GetAutoCompleteItems(string commandName);
    Task<int> RunCommand(string commandName, IReadOnlyList<string> argsToParse);
    Document GetOpenCliDocs();
}
