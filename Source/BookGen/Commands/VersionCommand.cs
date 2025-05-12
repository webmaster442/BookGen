//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;

namespace BookGen.Commands;

[CommandName("version")]
internal sealed class VersionCommand : Command
{
    public override int Execute(string[] context)
    {
        Version? version = typeof(VersionCommand).Assembly.GetName().Version;
        Console.WriteLine(version);
        return ExitCodes.Succes;
    }
}
