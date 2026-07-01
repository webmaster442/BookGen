//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli;

namespace BookGen.GlobalOptionParsers;

[Description("Enables logging to a file for the application.")]
internal class LogToFileParser : GlobalOptionParser
{
    private const string LogFileShort = "lf";
    private const string LogFileLong = "log-file";

    private readonly ProgramInfo _info;

    public LogToFileParser(ProgramInfo info) 
        : base(LogFileShort, LogFileLong)
    {
        _info = info;
    }

    protected override void OnOptionWasPresent()
    {
        _info.LogToFile = true;
    }
}
