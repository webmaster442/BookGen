//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using Bookgen.Lib.Domain.IO.Configuration;

using SkiaSharp;

using Svg.Skia;

namespace Bookgen.Lib.Rendering.Images;

internal static class ImageUtils
{
    public static byte[] ConvertToPng(string file, int maxwidth, int maxHeight)
    {
        if (Path.GetExtension(file).Equals(".svg", StringComparison.OrdinalIgnoreCase))
        {
            using FileStream stream = File.OpenRead(file);
            return RenderSvg(stream, maxwidth, maxHeight, SvgRecodeOption.AsPng).ToArray();
        }
        using SKBitmap bitmap = SKBitmap.Decode(file);
        using SKBitmap resized = ResizeIfBigger(bitmap, maxwidth, maxHeight);

        using SKData encoded = resized.Encode(SKEncodedImageFormat.Png, 100);

        return encoded.ToArray();
    }

    private static (int renderWidth, int renderHeight, float scale) CalcNewSize(SKRect size,
                                                                                int maxwidth,
                                                                                int maxHeight)
    {
        if (maxwidth < 0)
            maxwidth = (int)size.Width;

        if (maxHeight < 0)
            maxHeight = (int)size.Height;

        float scale = 1.0f;

        if (size.Height > maxHeight && maxwidth <= (int)size.Width)
        {
            scale = maxHeight / size.Height;
        }
        else if (size.Width > maxwidth && maxHeight <= (int)size.Height)
        {
            scale = maxwidth / size.Width;
        }
        else if (size.Width > maxwidth || size.Height > maxHeight)
        {
            float imgMax = Math.Max(size.Width, size.Height);
            float targetMin = Math.Min(maxwidth, maxHeight);
            scale = targetMin / imgMax;
        }

        return (renderWidth: (int)(size.Width * scale),
                renderHeight: (int)(size.Height * scale),
                scale);
    }


    public static SKBitmap ResizeIfBigger(SKBitmap input, int width, int height)
    {
        if (input.Width < width && input.Height < height)
            return input;

        (int renderWidth, int renderHeight, float scale) = CalcNewSize(new SKRect(0, 0, input.Width, input.Height), width, height);

        return input.Resize(new SKImageInfo(renderWidth, renderHeight), SKSamplingOptions.Default);
    }

    private static SKSvg LoadSvg(Stream stream)
    {
        var svg = new SKSvg();
        svg.Load(stream);
        return svg;
    }

    private static SKSvg LoadSvg(string svgData)
    {
        var svg = new SKSvg();
        using var xmlReader = System.Xml.XmlReader.Create(new StringReader(svgData));
        svg.Load(xmlReader);
        return svg;
    }

    private static SKData RenderSvg(SKSvg svg, int maxWidth, int maxHeight, SvgRecodeOption svgRecode)
    {
        if (svg.Picture == null)
            return SKData.Empty;

        SKRect svgSize = svg.Picture.CullRect;

        (int renderWidth, int renderHeight, float scale) = CalcNewSize(svgSize, maxWidth, maxHeight);

        var matrix = SKMatrix.CreateScale(scale, scale);

        using (var bitmap = new SKBitmap(renderWidth, renderHeight))
        {
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.DrawPicture(svg.Picture, matrix);
                canvas.Flush();
            }

            using (var image = SKImage.FromBitmap(bitmap))
            {
                return svgRecode switch
                {
                    SvgRecodeOption.AsPng => image.Encode(SKEncodedImageFormat.Png, 100),
                    SvgRecodeOption.AsWebp => image.Encode(SKEncodedImageFormat.Webp, 90),
                    SvgRecodeOption.Passtrough => throw new InvalidOperationException("Passthrough is an invalid render option here"),
                    _ => throw new UnreachableException(),
                };
            }
        }
    }

    public static SKData RenderSvg(Stream svgsource, int maxWidth, int maxHeight, SvgRecodeOption svgRecode)
    {
        using var svg = LoadSvg(svgsource);
        return RenderSvg(svg, maxWidth, maxHeight, svgRecode);
    }

    public static SKData RenderSvg(string svgData, int maxWidth, int maxHeight, SvgRecodeOption svgRecode)
    {
        using var svg = LoadSvg(svgData);
        return RenderSvg(svg, maxWidth, maxHeight, svgRecode);
    }

    public static SKData Encode(Stream fileData, int resizeWith, int resizeHeight, int quality, ImgRecodeOption imgRecodeOption)
    {
        using SKBitmap source = SKBitmap.Decode(fileData);
        using SKBitmap resized = ResizeIfBigger(source, resizeWith, resizeHeight);

        return resized.Encode(imgRecodeOption switch
        {
            ImgRecodeOption.AsWebp => SKEncodedImageFormat.Webp,
            ImgRecodeOption.AsPng => SKEncodedImageFormat.Png,
            ImgRecodeOption.Passtrough => throw new InvalidOperationException("Passthrough is an invalid encode option here"),
            _ => throw new UnreachableException(),
        }, quality);
    }
}
