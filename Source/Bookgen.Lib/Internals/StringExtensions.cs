//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Bookgen.Lib.Internals;

internal static partial class StringExtensions
{
    private static readonly Dictionary<char, string> SymbolNames = new()
    {
        { '#', "sharp" },
        { '|', "pipe" },
        { '@', "at" },
        { '&', "and" },
        { '~', "tide" },
        { '%', "percent" },
        { '+', "plus" },
        { '-', "minus" },
        { ':', "colon" },
        { ';', "semicolon" },
        { ',', "comma" },
        { '`', "backtick" },
        { '\'', "apoostrophe" },
        { '"', "quotationmark" },
        { '$', "dollar" },
        { '.', "dot" },
        { '!', "exclamation" },
        { '*', "star" },
        { '/', "slash" },
    };

    private static readonly HashSet<char> AllowedChars
        = new("abcdefghijklmnopqrstuvwxyz0123456789-_");

    public static string ToUrlNiceName(this string tag)
    {
        string ascii = AsciiEncodeLowerCase(tag);
        string replaced = ReplaceNotAllowedChars(ascii);
        if (string.IsNullOrEmpty(replaced))
            return "n-a";
        else
            return replaced;
    }

    private static string ReplaceNotAllowedChars(string input)
    {
        var final = new StringBuilder(input.Length);
        foreach (char c in input)
        {
            if (AllowedChars.Contains(c))
                final.Append(c);
            else if (SymbolNames.TryGetValue(c, out string? value))
                final.Append(value);
            else
                final.Append('-');
        }
        return final.ToString().Trim();
    }

    private static string AsciiEncodeLowerCase(string text)
    {
        if (text == null)
            return string.Empty;

        text = text.Replace("?", "question").Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(text.Length);

        foreach (var c in text)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        return sb.ToString()
            .Normalize(NormalizationForm.FormC)
            .ToLowerInvariant();
    }

    [GeneratedRegex(@"<(area|base|br|col|embed|hr|img|input|link|meta|param|source|track|wbr)(\s[^>/]*?)?>", RegexOptions.IgnoreCase)]
    private static partial Regex SelfCloser();

    public static string MakeSelfClosingTagsXmlCompatible(this string html)
        => SelfCloser().Replace(html, "<$1$2/>");
}
