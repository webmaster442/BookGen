//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

namespace BookGen.ShellHelper;

internal sealed class TerminalOutputBuilder
{
    private readonly StringBuilder _builder;

    public TerminalOutputBuilder()
    {
        _builder = new StringBuilder();
    }

    public enum ForegroundColor
    {
        Black = 30,
        Red = 31,
        Green = 32,
        Yellow = 33,
        Blue = 34,
        Magenta = 35,
        Cyan = 36,
        White = 37,
        Default = 39,
        Reset = 0,
    }

    public enum BackgroundColor
    {
        Black = 40,
        Red = 41,
        Green = 42,
        Yellow = 43,
        Blue = 44,
        Magenta = 45,
        Cyan = 46,
        White = 47,
        Default = 49,
        Reset = 0,
    }

    public TerminalOutputBuilder Append(ForegroundColor foreground, BackgroundColor? background, string text)
    {
        _builder.Append($"\x1b[{(int)foreground};{(int)(background ?? BackgroundColor.Default)}m{text}\x1b[0m");
        return this;
    }

    public override string ToString()
    {
        return _builder.ToString();
    }
}
