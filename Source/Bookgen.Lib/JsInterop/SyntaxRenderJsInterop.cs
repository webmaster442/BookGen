//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Vfs;

namespace Bookgen.Lib.JsInterop;

public sealed class SyntaxRenderJsInterop : JavascriptInterop
{
    private readonly IAssetSource _assetSource;
    private bool _prismLoaded;

    public SyntaxRenderJsInterop(IAssetSource assetSource)
    {
        _assetSource = assetSource;
    }

    public string PrismSyntaxHighlight(string code, string language)
    {
        if (!_prismLoaded)
        {
            string prismjs = _assetSource.GetAsset(BundledAssets.PrismJs);
            Execute(prismjs);
            _prismLoaded = true;
        }
        _engine.Script.code = code;
        return ExecuteAndGetResult($"Prism.highlight(code, Prism.languages.{language}, '{language}');");
    }

}
