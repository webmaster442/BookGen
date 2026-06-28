namespace Bookgen.Experiments;

// TODO: Add Replace function
// TODO: Add Regex Replace
// TODO: Add HtmlEncode function
// TODO: Add UrlEncode function
// TODO: Add CurrentDate function
// TODO: Add CurrentTime function
// TODO: Add UrlDecode function
// TODO: Add Base64Encode function
// TODO: Add Base64Decode function
internal static class BuiltinFunctions
{
    public static string ToUpper(object obj)
    {
        return obj is IFormattable formattable
            ? formattable.ToString(null, System.Globalization.CultureInfo.InvariantCulture).ToUpper()
            : obj?.ToString()?.ToUpper()
            ?? string.Empty;
    }

    public static string ToLower(object obj)
    {
        return obj is IFormattable formattable
            ? formattable.ToString(null, System.Globalization.CultureInfo.InvariantCulture).ToLower()
            : obj?.ToString()?.ToLower()
            ?? string.Empty;
    }

    public static string Substring(object obj, int startIndex, int length)
    {
        if (obj is string str)
            return str.Substring(startIndex, length);

        return obj?.ToString()?.Substring(startIndex, length) ?? string.Empty;
    }


    public static string Trim(object obj)
    {
        if (obj is string str)
            return str.Trim();

        return obj?.ToString()?.Trim() ?? string.Empty;
    }
}
