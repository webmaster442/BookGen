//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;

namespace BookGen.GlobalOptionParsers;

internal class RuntimePrintingParser : GlobalOptionParser
{
    private const string NoRuntimeShort = "-nr";
    private const string NoRuntimeLong = "--no-runtime";

    private readonly ProgramInfo _info;

    public RuntimePrintingParser(ProgramInfo info)
        : base(NoRuntimeShort, NoRuntimeLong)
    {
        _info = info;
        _info.PrintRuntime = true;
    }

    protected override void OnOptionWasPresent()
    {
        _info.PrintRuntime = false;
    }
}
