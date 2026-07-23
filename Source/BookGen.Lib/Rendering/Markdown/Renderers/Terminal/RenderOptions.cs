//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Webmaster442.WindowsTerminal;

namespace BookGen.Lib.Rendering.Markdown.Renderers.Terminal;

public sealed class RenderOptions
{
    public TerminalColor HeadingColor { get; set; }

    public TerminalColor LinkColor { get; set; }

    public TerminalColor CodeInlineColor { get; set; }

    public TerminalColor QuoteBlockColor { get; set; }

    public TerminalColor CodeBlockBackground { get; set; }

    public TerminalColor CodeBlockColor { get; set; }

    public int Width
    {
        get => field;
        set
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException(nameof(Width), "Width must be greater than 0.");

            field = value;
        }
    }

    public RenderOptions()
    {
        HeadingColor = TerminalColor.Green;
        LinkColor = TerminalColor.BrightBlue;
        CodeInlineColor = TerminalColor.BrightRed;
        QuoteBlockColor = TerminalColor.BrightWhite;
        CodeBlockColor = TerminalColor.Black;
        CodeBlockBackground = TerminalColor.White;
        Width = 120;
    }
}
