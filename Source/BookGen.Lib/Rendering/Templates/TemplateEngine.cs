using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Lib.Rendering.Templates;

public sealed class TemplateEngine : GenericTemplateEngine<ViewData>
{
    private readonly IAssetSource _assetSource;

    public TemplateEngine(ILogger logger, IAssetSource assetSource) : this(logger, assetSource, new TemplateEngineOptions())
    {
    }

    public TemplateEngine(ILogger logger, IAssetSource assetSource, TemplateEngineOptions options) : base(logger, options)
    {
        _assetSource = assetSource;
        var functions = new TemplateFunctions(options.TimeProvider);
        RegisterFunction(nameof(functions.ToUpper), functions.ToUpper);
        RegisterFunction(nameof(functions.ToLower), functions.ToLower);
        RegisterFunction(nameof(functions.Substring), functions.Substring);
        RegisterFunction(nameof(functions.Trim), functions.Trim);
        RegisterFunction(nameof(functions.TrimStart), functions.TrimStart);
        RegisterFunction(nameof(functions.TrimEnd), functions.TrimEnd);
        RegisterFunction(nameof(functions.Replace), functions.Replace);
        RegisterFunction(nameof(functions.Concat), functions.Concat);
        RegisterFunction(nameof(functions.RegexReplace), functions.RegexReplace);
        RegisterFunction(nameof(functions.HtmlEncode), functions.HtmlEncode);
        RegisterFunction(nameof(functions.UrlEncode), functions.UrlEncode);
        RegisterFunction(nameof(functions.CurrentDate), functions.CurrentDate);
        RegisterFunction(nameof(functions.CurrentDateFormat), functions.CurrentDateFormat);
        RegisterFunction(nameof(functions.CurrentDateTime), functions.CurrentDateTime);
        RegisterFunction(nameof(functions.CurrentDateTimeFormat), functions.CurrentDateTimeFormat);
        RegisterFunction(nameof(functions.CurrentTime), functions.CurrentTime);
        RegisterFunction(nameof(functions.CurrentTimeFormat), functions.CurrentTimeFormat);
        RegisterFunction(nameof(functions.UrlDecode), functions.UrlDecode);
    }
}
