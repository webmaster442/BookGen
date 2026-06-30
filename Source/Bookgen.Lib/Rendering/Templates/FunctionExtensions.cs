//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Globalization;
using System.Text;

namespace Bookgen.Lib.Rendering.Templates;

internal static class FunctionExtensions
{
    public static TValue GetValueOrDefault<TValue>(this IReadOnlyList<string> arguments,
                                                   int index,
                                                   TValue defaultValue)
        where TValue : IParsable<TValue>
    {
        if (index < 0 || index >= arguments.Count)
            return defaultValue;
        var value = arguments[index];

        if (TValue.TryParse(value, CultureInfo.InvariantCulture, out TValue? parsedValue))
            return parsedValue;

        return defaultValue;
    }

    public static string ToJavacriptHtmlElement(this string code)
    {
        var sb = new StringBuilder(code.Length + 10);
        sb.Append("<script type=\"text/javascript\">")
          .AppendLine(code)
          .Append("</script>");
        return sb.ToString();
    }

}
