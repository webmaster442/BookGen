using BookGen.Vfs;

namespace Bookgen.Lib.JsInterop;

public sealed class MathJaxInterop : JavascriptInterop
{
    private readonly Func<string, double, string> TypeSet;

    public MathJaxInterop(IAssetSource assetSource)
    {
        string mathJax = assetSource.GetAsset(BundledAssets.MathJax);
        Execute(mathJax);
        TypeSet = (Func<string, double, string>)Evaluate("MathJaxModule.typeset");
    }

    public string RenderLatexToSvg(string latex, double scale = 1.0)
    {
        return TypeSet(latex, scale);
    }
}
