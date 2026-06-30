//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;

namespace BookGen.GlobalOptionParsers;

internal class JsonLogParser : GlobalOptionParser
{
    private const string JsonLogShort = "-js";
    private const string JsonLogLong = "--json-log";

    private readonly ProgramInfo _info;

    public JsonLogParser(ProgramInfo info)
        : base(JsonLogShort, JsonLogLong)
    {
        _info = info;
    }

    protected override void OnOptionWasPresent()
    {
        _info.JsonLogging = true;
    }
}
