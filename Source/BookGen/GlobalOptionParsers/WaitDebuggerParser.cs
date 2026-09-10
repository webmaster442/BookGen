//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics;

using BookGen.Cli;

using Microsoft.Extensions.Logging;

using Spectre.Console;

namespace BookGen.GlobalOptionParsers;

[Description("Waits for a debugger to be attached before continuing execution.")]
internal sealed class WaitDebuggerParser : GlobalOptionParser
{
    private const string DebuggerShort = "wd";
    private const string DebuggerLong = "wait-debugger";
    private readonly ILogger _log;

    public WaitDebuggerParser(ILogger log)
        : base(DebuggerShort, DebuggerLong)
    {
        _log = log;
    }

    protected override void OnOptionWasPresent(string value)
    {
        _log.LogInformation("Waiting for debugger to be attached...");
        _log.LogInformation("ESC to cancel & continue execution...");
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
