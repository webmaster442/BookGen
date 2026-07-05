//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Infrastructure;

internal interface IHelpProvider
{
    IEnumerable<string> HelpEntries { get; }
    IEnumerable<string> GetCommandHelp(string cmd);
    void RegisterCallback(string commandName, Func<string> callback);
}
