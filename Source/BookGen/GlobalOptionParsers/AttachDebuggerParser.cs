//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics;

using BookGen.Cli;

using Microsoft.Extensions.Logging;

namespace BookGen.GlobalOptionParsers;

[Description("Attaches a debugger to the process if one is not already attached.")]
internal sealed class AttachDebuggerParser : GlobalOptionParser
{
    private const string DebuggerStartShort = "ad";
    private const string DebuggerStartLong = "attach-debugger";
    private readonly ILogger _log;

    public AttachDebuggerParser(ILogger log)
        : base(DebuggerStartShort, DebuggerStartLong)
    {
        _log = log;
    }

    protected override void OnOptionWasPresent(string value)
    {
        _log.LogInformation("Attaching debugger...");
        if (!Debugger.IsAttached)
        {
            Debugger.Launch();
        }
    }
}
