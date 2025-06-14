using System.Globalization;
using System.Text;

using BookGen.Vfs;

namespace Bookgen.Lib.Templates;

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

        if (TValue.TryParse(value, CultureInfo.InvariantCulture, out var parsedValue))
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

public sealed class DefaultFunctions
{
    private readonly IAssetSource _assetSource;

    public DefaultFunctions(IAssetSource assetSource)
    {
        _assetSource = assetSource;
    }

    public string BuildDate(string[] arguments)
        => DateTime.Now.ToString(arguments.GetValueOrDefault(0, "yy-MM-dd hh:mm:ss"));

    public string JSPageToc(string[] arguments)
    {
        string contentsDiv = arguments.GetValueOrDefault(0, string.Empty);
        string targetDiv = arguments.GetValueOrDefault(1, string.Empty);

        if (string.IsNullOrEmpty(contentsDiv) || string.IsNullOrEmpty(targetDiv))
            throw new ArgumentException("JSPageToc requires two arguments: contentsDiv and targetDiv.");

        var pagetoc = _assetSource.GetAsset(BundledAssets.JsPageToc);

        string code = pagetoc.Replace("{{contents}}", contentsDiv).Replace("{{target}}", targetDiv);

        return code.ToJavacriptHtmlElement();
    }
}
