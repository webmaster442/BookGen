//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.ImageService;

using BookGen.Vfs;

using SkiaSharp;

namespace Bookgen.Lib.JsInterop;

public sealed class ImageRenderJsInterop : JavascriptInterop
{
    private readonly IAssetSource _assetSource;
    private readonly ImageConfig _imageConfig;
    private bool _nomnomlLoaded;

    public ImageRenderJsInterop(IAssetSource assetSource, ImageConfig imageConfig)
    {
        _assetSource = assetSource;
        _imageConfig = imageConfig;
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

    private ImageResult EncodeSvg(string svgData)
    {
        if (_imageConfig.SvgRecode == SvgRecodeOption.Passtrough)
        {
            return new ImageResult
            {
                Data = svgData,
                ImageType = ImageType.Svg,
                OriginalName = string.Empty,
            };
        }



        using SKData rendered = ImageUtils.RenderSvg(svgData,
                                                     _imageConfig.ResizeWith,
                                                     _imageConfig.ResizeHeight,
                                                     _imageConfig.SvgRecode);

        return new ImageResult
        {
            Data = Convert.ToBase64String(rendered.AsSpan()),
            ImageType = GetImateType(_imageConfig.SvgRecode),
            OriginalName = string.Empty
        };
    }

    public ImageResult RenderNomnoml(string nomnomlCode)
    {
        if (!_nomnomlLoaded)
        {
            Execute(_assetSource.GetAsset(BundledAssets.GraphreJs));
            Execute(_assetSource.GetAsset(BundledAssets.NomnomlJs));
            _nomnomlLoaded = true;
        }

        _engine.Script.nomnomlCode = nomnomlCode;
        string svg = ExecuteAndGetResult("nomnoml.renderSvg(nomnomlCode)");
        return EncodeSvg(svg);
    }
}
