//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;

namespace BookGen.GlobalOptionParsers;

internal class LogToFileParser : GlobalOptionParser
{
    private const string LogFileShort = "-lf";
    private const string LogFileLong = "--log-file";

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
