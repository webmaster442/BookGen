//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using BookGen.Cli;

using Spectre.Console;

namespace BookGen.GlobalOptionParsers;

internal sealed class AttachDebuggerParser : GlobalOptionParser
{
    private const string DebuggerStartShort = "-ad";
    private const string DebuggerStartLong = "--attach-debugger";

    public AttachDebuggerParser() 
        : base(DebuggerStartShort, DebuggerStartLong)
    {
    }

    protected override void OnOptionWasPresent()
    {
        AnsiConsole.WriteLine("Attaching debugger...");
        if (!Debugger.IsAttached)
        {
            Debugger.Launch();
        }
    }
}
