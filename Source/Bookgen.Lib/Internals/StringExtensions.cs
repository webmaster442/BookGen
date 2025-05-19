using System.Text;

namespace Bookgen.Lib.Internals;

internal static class StringExtensions
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
        var allowed = new HashSet<char>("abcdefghijklmnopqrstuvwxyz0123456789-_");
        var final = new StringBuilder(input.Length);
        foreach (char c in input)
        {
            if (allowed.Contains(c))
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
        text = text.Replace("?", "question");
        var ascii = new ASCIIEncoding();
        byte[] byteArray = Encoding.UTF8.GetBytes(text.Normalize(NormalizationForm.FormD));
        byte[] asciiArray = Encoding.Convert(Encoding.UTF8, Encoding.ASCII, byteArray);
        return ascii.GetString(asciiArray).Replace("?", "").ToLower();
    }
}
