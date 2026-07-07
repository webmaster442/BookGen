//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli.OpenCli.Draft;

namespace BookGen.Cli;

public interface ICommandHelpProvider
{
    void CommandsChanged(Document openCliDocument);
    string GetHelp(string commandName);
}
