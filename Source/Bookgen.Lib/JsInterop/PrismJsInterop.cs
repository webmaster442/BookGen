using Bookgen.Lib.Pipeline;

namespace Bookgen.Lib.JsInterop;

public sealed class PrismJsInterop : JavascriptInterop
{
    public PrismJsInterop(IBookEnvironment environment)
    {
        if (!environment.TryGetAsset(BundledAssets.PrismJs, out string? prismjs))
            throw new InvalidOperationException($"{BundledAssets.PrismJs} not found");

        Execute(prismjs);
    }

    public string PrismSyntaxHighlight(string code, string language)
    {
        _engine.Script.code = code;
        return ExecuteAndGetResult($"Prism.highlight(code, Prism.languages.{language}, '{language}');");
    }

}
