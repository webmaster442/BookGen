using System.Diagnostics;
using System.Web;

using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.ImageService;

using BookGen.Vfs;

using SkiaSharp;

namespace Bookgen.Lib.Markdown.RenderInterop;

public sealed class RenderInterop : IRenderInterop
{
    private readonly IAssetSource _assetSource;
    private readonly JavascriptEngine _javascriptEngine;
    private readonly HashSet<string> _loadedScripts;
    private bool _disposed;

    private readonly ImageConfig _imageConfig;

    public RenderInterop(IAssetSource assetSource, ImageConfig imageConfig)
    {
        _assetSource = assetSource;
        _imageConfig = imageConfig;
        _javascriptEngine = new JavascriptEngine();
        _loadedScripts = new HashSet<string>();
    }

    public void Dispose()
    {
        _javascriptEngine.Dispose();
        _disposed = true;
    }

    private void LoadScriptIfNotLoaded(string scriptFile)
    {
        if (_loadedScripts.Contains(scriptFile))
        {
            return;
        }

        string script = _assetSource.GetAsset(scriptFile);
        _javascriptEngine.Execute(script);
        _loadedScripts.Add(scriptFile);
    }

    private static ImageType GetImateType(SvgRecodeOption recodeOption)
    {
        return recodeOption switch
        {
            SvgRecodeOption.AsPng => ImageType.Png,
            SvgRecodeOption.AsWebp => ImageType.Webp,
            SvgRecodeOption.Passtrough => ImageType.Svg,
            _ => throw new UnreachableException(),
        };
    }

    private static ImageResult EncodeSvg(string svgData, ImageConfig imageConfig)
    {
        if (imageConfig.SvgRecode == SvgRecodeOption.Passtrough)
        {
            return new ImageResult
            {
                Data = svgData,
                ImageType = ImageType.Svg,
                OriginalName = string.Empty,
            };
        }



        using SKData rendered = ImageUtils.RenderSvg(svgData,
                                                     imageConfig.ResizeWith,
                                                     imageConfig.ResizeHeight,
                                                     imageConfig.SvgRecode);

        return new ImageResult
        {
            Data = Convert.ToBase64String(rendered.AsSpan()),
            ImageType = GetImateType(imageConfig.SvgRecode),
            OriginalName = string.Empty
        };
    }

    public bool PreRenderCode { get; set; } = true;

    public string PrismSyntaxHighlight(string code, string language)
    {
        if (PreRenderCode)
        {
            ObjectDisposedException.ThrowIf(_disposed, nameof(RenderInterop));
            LoadScriptIfNotLoaded(BundledAssets.PrismJs);

            _javascriptEngine.Script.code = code;
            return _javascriptEngine.ExecuteAndGetResult($"Prism.highlight(code, Prism.languages.{language}, '{language}');");
        }

        return HttpUtility.HtmlEncode(code);
    }

    public ImageResult RenderLatex(string latex, double scale = 1.0)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(RenderInterop));

        return EncodeSvg(ProcessInterop.RunRatex(latex, scale), _imageConfig);

    }

    public ImageResult RenderNomnoml(string nomnomlCode)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(RenderInterop));
        LoadScriptIfNotLoaded(BundledAssets.GraphreJs);
        LoadScriptIfNotLoaded(BundledAssets.NomnomlJs);

        _javascriptEngine.Script.nomnomlCode = nomnomlCode;
        string svg = _javascriptEngine.ExecuteAndGetResult("nomnoml.renderSvg(nomnomlCode)");
        return EncodeSvg(svg, _imageConfig);
    }

    public ImageResult RenderQrCode(string url)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(RenderInterop));
        LoadScriptIfNotLoaded(BundledAssets.QrCodeJs);

        string cmd = $"new QRCode({{content: \"{url}\", padding: 2, color: \"#000000\"}}).svg();";
        string svg = _javascriptEngine.ExecuteAndGetResult(cmd);
        return EncodeSvg(svg, _imageConfig);
    }
}
