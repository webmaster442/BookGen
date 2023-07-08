//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

namespace BookGen.ShellHelper;
internal sealed class TerminalStringBuilder
{
    private readonly StringBuilder _builder;

    public TerminalStringBuilder()
    {
        _builder = new StringBuilder();
    }

    public TerminalStringBuilder Default() 
    {
        _builder.Append("\x1b[0m");
        return this;
    }

    public TerminalStringBuilder ForegroundBlack() 
    {
        _builder.Append("\x1b[30m");
        return this;
    }

    public TerminalStringBuilder BackgroundGreen()
    {
        _builder.Append("\x1b[42m");
        return this;
    }

    public TerminalStringBuilder BackgroundMagenta()
    {
        _builder.Append("\x1b[45m");
        return this;
    }

    public TerminalStringBuilder BackgroundYellow()
    {
        _builder.Append("\x1b[43m");
        return this;
    }

    public TerminalStringBuilder BackgroundCyan()
    {
        _builder.Append("\x1b[46m");
        return this;
    }

    public TerminalStringBuilder Text(object o)
    {
        _builder.Append(o);
        return this;
    }

    public override string ToString()
    {
        return _builder.ToString();
    }
}
