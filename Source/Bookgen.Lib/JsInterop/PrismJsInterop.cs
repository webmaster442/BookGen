using BookGen.Vfs;

namespace Bookgen.Lib.JsInterop;

public sealed class PrismJsInterop : JavascriptInterop
{
    public PrismJsInterop(IAssetSource assetSource)
    {
        string prismjs = assetSource.GetAsset(BundledAssets.PrismJs);
        Execute(prismjs);
    }

    public string PrismSyntaxHighlight(string code, string language)
    {
        _engine.Script.code = code;
        return ExecuteAndGetResult($"Prism.highlight(code, Prism.languages.{language}, '{language}');");
    }

}
