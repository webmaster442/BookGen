using System.Diagnostics;
using System.Text;

using Bookgen.Lib.Domain.IO.Configuration;

using SkiaSharp;

using Svg.Skia;

namespace Bookgen.Lib.ImageService;

internal static class Utils
{
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


    private static SKBitmap ResizeIfBigger(SKBitmap input, int width, int height)
    {
        if (input.Width < width && input.Height < height)
            return input;

        (int renderWidth, int renderHeight, float scale) = CalcNewSize(new SKRect(0, 0, input.Width, input.Height), width, height);

        return input.Resize(new SKImageInfo(renderWidth, renderHeight), SKSamplingOptions.Default);
    }

    public static SKData RenderSvg(Stream stream, int width, int height, SvgRecodeOption svgRecode)
    {
        using var svg = new SKSvg();
        svg.Load(stream);

        if (svg.Picture == null)
            return SKData.Empty;

        SKRect svgSize = svg.Picture.CullRect;

        (int renderWidth, int renderHeight, float scale) = CalcNewSize(svgSize, width, height);

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

    public static SKData EncodeToWebp(Stream fileData, int resizeWith, int resizeHeight, int quality)
    {
        using SKBitmap source = SKBitmap.Decode(fileData);
        using SKBitmap resized = ResizeIfBigger(source, resizeWith, resizeHeight);
        return resized.Encode(SKEncodedImageFormat.Webp, quality);
    }

    public static string Base64Enode(Stream fileData)
    {
        StringBuilder builder = new StringBuilder();
        byte[] buffer = new byte[4096];
        int read = 0;
        do
        {
            read = fileData.Read(buffer, 0, buffer.Length);
            builder.Append(Convert.ToBase64String(buffer, 0, read));
        }
        while (read > 0);
        return builder.ToString();
    }
}
