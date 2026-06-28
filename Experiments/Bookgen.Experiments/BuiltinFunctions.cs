using System.Text;
using System.Text.RegularExpressions;

namespace Bookgen.Experiments;

public static class BuiltinFunctions
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

    private static string ToUpper(object obj)
        => ToString(obj).ToUpper();

    private static string ToLower(object obj)
        => ToString(obj).ToLower();

    private static string Substring(object obj, object startIndex, object length)
        => ToString(obj).Substring(ToInt(startIndex), ToInt(length));

    private static string Trim(object obj)
        => ToString(obj).Trim();

    private static string TrimStart(object obj)
        => ToString(obj).TrimStart();

    private static string TrimEnd(object obj)
        => ToString(obj).TrimEnd();

    private static string Replace(object obj, object oldValue, object newValue)
        => ToString(obj).Replace(ToString(oldValue), ToString(newValue));

    private static string Concat(object[] args)
    {
        StringBuilder result = new StringBuilder();
        foreach (object arg in args)
        {
            result.Append(ToString(arg));
        }
        return result.ToString();
    }

    private static string RegexReplace(object obj, object pattern, object replacement)
    {
        var input = ToString(obj);
        var regexPattern = ToString(pattern);
        var replacementStr = ToString(replacement);
        return Regex.Replace(input, regexPattern, replacementStr, RegexOptions.CultureInvariant, TimeSpan.FromSeconds(5));
    }

    private static string HtmlEncode(object obj)
    {
        var input = ToString(obj);
        return System.Net.WebUtility.HtmlEncode(input);
    }

    private static string UrlEncode(object obj)
    {
        var input = ToString(obj);
        return System.Net.WebUtility.UrlEncode(input);
    }

    private static string CurrentDate()
        => DateTime.Now.ToString("yyyy-MM-dd");

    private static string CurrentDateFormat(object format)
        => DateTime.Now.ToString(ToString(format));

    private static string CurrentTime()
        => DateTime.Now.ToString("HH:mm:ss");

    private static string CurrentTimeFormat(object format)
        => DateTime.Now.ToString(ToString(format));

    private static string CurrentDateTime()
        => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    private static string CurrentDateTimeFormat(object format)
        => DateTime.Now.ToString(ToString(format));

    private static string UrlDecode(object obj)
    {
        var input = ToString(obj);
        return System.Net.WebUtility.UrlDecode(input);
    }

    public static void RegisterBuiltinFunctions<T>(this TemplateEngine<T> engine)
    {
        engine.RegisterFunction(nameof(ToUpper), ToUpper);
        engine.RegisterFunction(nameof(ToLower), ToLower);
        engine.RegisterFunction(nameof(Substring), Substring);
        engine.RegisterFunction(nameof(Trim), Trim);
        engine.RegisterFunction(nameof(TrimStart), TrimStart);
        engine.RegisterFunction(nameof(TrimEnd), TrimEnd);
        engine.RegisterFunction(nameof(Replace), Replace);
        engine.RegisterFunction(nameof(Concat), Concat);
        engine.RegisterFunction(nameof(RegexReplace), RegexReplace);
        engine.RegisterFunction(nameof(HtmlEncode), HtmlEncode);
        engine.RegisterFunction(nameof(UrlEncode), UrlEncode);
        engine.RegisterFunction(nameof(CurrentDate), CurrentDate);
        engine.RegisterFunction(nameof(CurrentDate), CurrentDateFormat);
        engine.RegisterFunction(nameof(CurrentDateTime), CurrentDateTime);
        engine.RegisterFunction(nameof(CurrentDateTime), CurrentDateTimeFormat);
        engine.RegisterFunction(nameof(CurrentTime), CurrentTime);
        engine.RegisterFunction(nameof(CurrentTime), CurrentTimeFormat);
        engine.RegisterFunction(nameof(UrlDecode), UrlDecode);
    }
}
