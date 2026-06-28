using System.Text;
using System.Text.RegularExpressions;

namespace Bookgen.Experiments;

internal sealed class BuiltinFunctions(TimeProvider timeProvider)
{
    private static string ToString(object? obj)
    {
        if (obj == null)
            return string.Empty;

        if (obj is string str)
            return str;

        if (obj is IFormattable formattable)
            return formattable.ToString(null, System.Globalization.CultureInfo.InvariantCulture);

        return obj.ToString() ?? string.Empty;
    }

    private static int ToInt(object obj)
    {
        if (obj is int i)
            return i;

        if (obj is IConvertible convertible)
            return convertible.ToInt32(System.Globalization.CultureInfo.InvariantCulture);

        throw new InvalidCastException($"Cannot convert object of type {obj.GetType()} to int.");
    }

    internal string ToUpper(object obj)
        => ToString(obj).ToUpper();

    internal string ToLower(object obj)
        => ToString(obj).ToLower();

    internal string Substring(object obj, object startIndex, object length)
        => ToString(obj).Substring(ToInt(startIndex), ToInt(length));

    internal string Trim(object obj)
        => ToString(obj).Trim();

    internal string TrimStart(object obj)
        => ToString(obj).TrimStart();

    internal string TrimEnd(object obj)
        => ToString(obj).TrimEnd();

    internal string Replace(object obj, object oldValue, object newValue)
        => ToString(obj).Replace(ToString(oldValue), ToString(newValue));

    internal string Concat(object[] args)
    {
        StringBuilder result = new StringBuilder();
        foreach (object arg in args)
        {
            result.Append(ToString(arg));
        }
        return result.ToString();
    }

    internal string RegexReplace(object obj, object pattern, object replacement)
    {
        var input = ToString(obj);
        var regexPattern = ToString(pattern);
        var replacementStr = ToString(replacement);
        return Regex.Replace(input, regexPattern, replacementStr, RegexOptions.CultureInvariant, TimeSpan.FromSeconds(5));
    }

    internal string HtmlEncode(object obj)
    {
        var input = ToString(obj);
        return System.Net.WebUtility.HtmlEncode(input);
    }

    internal string UrlEncode(object obj)
    {
        var input = ToString(obj);
        return System.Net.WebUtility.UrlEncode(input);
    }

    internal string CurrentDate()
        => timeProvider.GetLocalNow().ToString("yyyy-MM-dd");

    internal string CurrentDateFormat(object format)
        => timeProvider.GetLocalNow().ToString(ToString(format));

    internal string CurrentTime()
        => timeProvider.GetLocalNow().ToString("HH:mm:ss");

    internal string CurrentTimeFormat(object format)
        => timeProvider.GetLocalNow().ToString(ToString(format));

    internal string CurrentDateTime()
        => timeProvider.GetLocalNow().ToString("yyyy-MM-dd HH:mm:ss");

    internal string CurrentDateTimeFormat(object format)
        => timeProvider.GetLocalNow().ToString(ToString(format));

    internal string UrlDecode(object obj)
    {
        var input = ToString(obj);
        return System.Net.WebUtility.UrlDecode(input);
    }
}
