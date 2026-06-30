//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using BookGen.Cli;

using Spectre.Console;

namespace BookGen.GlobalOptionParsers;

internal sealed class WaitDebuggerParser : GlobalOptionParser
{
    private const string DebuggerShort = "-wd";
    private const string DebuggerLong = "--wait-debugger";

    public WaitDebuggerParser() 
        : base(DebuggerShort, DebuggerLong)
    {
    }

    protected override void OnOptionWasPresent()
    {
        AnsiConsole.WriteLine("Waiting for debugger to be attached...");
        AnsiConsole.WriteLine("ESC to cancel & contine execution...");
        while (!Debugger.IsAttached)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
            else
            {
                Thread.Sleep(100);
            }
        }
        Debugger.Break();
        //Now you can debug the code
    }
}
