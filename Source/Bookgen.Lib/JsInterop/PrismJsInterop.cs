using Bookgen.Lib.Pipeline;

namespace Bookgen.Lib.JsInterop;

internal sealed class PrismJsInterop : JavascriptInterop
{
    private const string PrismJs = "prism.js";

    public PrismJsInterop(IEnvironment environment)
    {
        if (!environment.TryGetAsset(PrismJs, out string? prismjs))
            throw new InvalidOperationException($"{PrismJs} not found");

        Execute(prismjs);
    }

    public string PrismSyntaxHighlight(string code, string language)
    {
        _engine.Script.code = code;
        return ExecuteAndGetResult($"Prism.highlight(code, Prism.languages.{language}, '{language}');");
    }

}
