//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Shell.Shared;

public sealed class TerminalCell
{
    public TerminalColor Foreground { get; set; } = TerminalColor.Default;
    public TerminalColor Background { get; set; } = TerminalColor.Default;
    public string Text { get; set; } = string.Empty;

    public static implicit operator string (TerminalCell cell)
    {
        int foregroundCode = cell.Foreground == TerminalColor.Reset
            ? (int)cell.Foreground
            : (int)cell.Foreground + 20;

        int backgroundCode = cell.Background == TerminalColor.Reset 
            ? (int)cell.Background 
            : (int)cell.Background + 30;

        if (cell.Foreground == TerminalColor.Default 
            && cell.Background == TerminalColor.Default)
        {
            return cell.Text;
        }
        else if (cell.Foreground == TerminalColor.Default)
        {
            return $"\e[{backgroundCode}m{cell.Text}\e[0m";
        }
        else if (cell.Background == TerminalColor.Default)
        {
            return $"\e[{foregroundCode}m{cell.Text}\e[0m";
        }
        else
        {
            return $"\e[{foregroundCode};{backgroundCode}m{cell.Text}\e[0m";
        }
    }
}
