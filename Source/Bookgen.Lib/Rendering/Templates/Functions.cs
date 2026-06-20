//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Vfs;

namespace Bookgen.Lib.Rendering.Templates;

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
