//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

namespace Bookgen.Lib.Markdown.Renderers;

internal static partial class TerminalRenderer
{
    private static readonly Dictionary<string, string> staticTable = new()
    {
        // Bold
        ["\\e[1m"] = "<span style=\"font-weight:bold;\">",

        // Italic
        ["\\e[3m"] = "<span style=\"font-style:italic;\">",

        // Underline
        ["\\e[4m"] = "<span style=\"text-decoration:underline;\">",

        // Foreground colors
        ["\\e[30m"] = "<span style=\"color:black;\">",
        ["\\e[31m"] = "<span style=\"color:red;\">",
        ["\\e[32m"] = "<span style=\"color:green;\">",
        ["\\e[33m"] = "<span style=\"color:yellow;\">",
        ["\\e[34m"] = "<span style=\"color:blue;\">",
        ["\\e[35m"] = "<span style=\"color:magenta;\">",
        ["\\e[36m"] = "<span style=\"color:cyan;\">",
        ["\\e[37m"] = "<span style=\"color:white;\">",

        // Bright foreground colors
        ["\\e[90m"] = "<span style=\"color:gray;\">",
        ["\\e[91m"] = "<span style=\"color:#ff5555;\">",
        ["\\e[92m"] = "<span style=\"color:#50fa7b;\">",
        ["\\e[93m"] = "<span style=\"color:#f1fa8c;\">",
        ["\\e[94m"] = "<span style=\"color:#bd93f9;\">",
        ["\\e[95m"] = "<span style=\"color:#ff79c6;\">",
        ["\\e[96m"] = "<span style=\"color:#8be9fd;\">",
        ["\\e[97m"] = "<span style=\"color:#ffffff;\">",

        // Background colors
        ["\\e[40m"] = "<span style=\"background-color:black;\">",
        ["\\e[41m"] = "<span style=\"background-color:red;\">",
        ["\\e[42m"] = "<span style=\"background-color:green;\">",
        ["\\e[43m"] = "<span style=\"background-color:yellow;\">",
        ["\\e[44m"] = "<span style=\"background-color:blue;\">",
        ["\\e[45m"] = "<span style=\"background-color:magenta;\">",
        ["\\e[46m"] = "<span style=\"background-color:cyan;\">",
        ["\\e[47m"] = "<span style=\"background-color:white;\">",

        // Bright background colors
        ["\\e[100m"] = "<span style=\"background-color:gray;\">",
        ["\\e[101m"] = "<span style=\"background-color:#ff5555;\">",
        ["\\e[102m"] = "<span style=\"background-color:#50fa7b;\">",
        ["\\e[103m"] = "<span style=\"background-color:#f1fa8c;\">",
        ["\\e[104m"] = "<span style=\"background-color:#bd93f9;\">",
        ["\\e[105m"] = "<span style=\"background-color:#ff79c6;\">",
        ["\\e[106m"] = "<span style=\"background-color:#8be9fd;\">",
        ["\\e[107m"] = "<span style=\"background-color:#ffffff;\">",
    };

    public static string RenderAnsiCode(string input)
    {
        StringBuilder result = new(input.Length * 2);
        StringBuilder currentCode = new(10);
        bool inEscapeSequence = false;
        int spanOpened = 0;
        foreach (var chr in input)
        {
            if (chr == '\\')
            {
                currentCode.Append(chr);
                inEscapeSequence = true;
            }
            else if (chr == 'm' && inEscapeSequence)
            {
                currentCode.Append(chr);
                inEscapeSequence = false;
                string escapeCode = currentCode.ToString();
                if (escapeCode == "\\e[0m")
                {
                    while (spanOpened > 0)
                    {
                        result.Append("</span>");
                        --spanOpened;
                    }
                    currentCode.Clear();
                    continue;
                }

                bool rederResult = TryRender(escapeCode, out string rendered);

                result.Append(rendered);

                if (rederResult)
                {
                    ++spanOpened;
                }
                currentCode.Clear();
            }
            else if (inEscapeSequence)
            {
                currentCode.Append(chr);
            }
            else
            {
                result.Append(chr);
            }
        }
        return result.ToString();
    }

    private static bool TryRender(string escapeCode, out string rendered)
    {
        if (staticTable.TryGetValue(escapeCode, out string? value))
        {
            rendered = value;
            return true;
        }

        // Handle 24-bit color codes
        if (escapeCode.StartsWith("\\e[38;2;") || escapeCode.StartsWith("\\e[48;2;"))
        {
            var parts = escapeCode.Split(';');
            if (parts.Length == 5)
            {
                int r = int.Parse(parts[2]);
                int g = int.Parse(parts[3]);
                int b = int.Parse(parts[4].TrimEnd('m'));

                if (escapeCode.StartsWith("\\e[38;2;"))
                    rendered = $"<span style=\"color: rgb({r}, {g}, {b});\">"; // For foreground
                else
                    rendered = $"<span style=\"background-color: rgb({r}, {g}, {b});\">"; // For background

                return true;
            }
        }

        rendered = escapeCode;
        return false;
    }
}
