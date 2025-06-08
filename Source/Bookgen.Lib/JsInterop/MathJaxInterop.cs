using BookGen.Vfs;

namespace Bookgen.Lib.JsInterop;

public sealed class MathJaxInterop : JavascriptInterop
{
    private readonly dynamic TypeSet;

    public MathJaxInterop(IAssetSource assetSource)
    {
        string mathJax = assetSource.GetAsset(BundledAssets.MathJax);
        Execute(mathJax);
        TypeSet = Evaluate("MathJaxModule.typeset");
    }

    public string RenderLatexToSvg(string latex)
    {
        return TypeSet(latex);
    }
}
