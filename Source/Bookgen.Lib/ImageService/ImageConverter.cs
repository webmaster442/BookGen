//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Domain.IO.Configuration;

using SkiaSharp;

namespace Bookgen.Lib.ImageService;

public static class ImageConverter
{
    public static void Encode(string source, string output, ImageType imageType, int maxWidth, int maxHeight, int quality)
    {
        using FileStream srcStream = File.OpenRead(source);
        using FileStream destStream = File.Create(output);

        if (Path.GetExtension(source).Equals(".svg", StringComparison.OrdinalIgnoreCase))
        {
            SKData img = ImageUtils.RenderSvg(srcStream, maxWidth, maxHeight, GetRecodeOption(imageType));
            img.SaveTo(destStream);
            return;
        }

        using SKBitmap loaded = SKBitmap.Decode(srcStream);
        using SKBitmap result = ImageUtils.ResizeIfBigger(loaded, maxWidth, maxHeight);

        result.Encode(destStream, imageType switch
        {
            ImageType.Png => SKEncodedImageFormat.Png,
            ImageType.Jpeg => SKEncodedImageFormat.Jpeg,
            ImageType.Webp => SKEncodedImageFormat.Webp,
            _ => throw new NotSupportedException($"Image type {imageType} is not supported for encoding.")
        }, quality);
    }

    private static SvgRecodeOption GetRecodeOption(ImageType imageType)
    {
        return imageType switch
        {
            ImageType.Png => SvgRecodeOption.AsPng,
            ImageType.Webp => SvgRecodeOption.AsWebp,
            _ => throw new NotSupportedException($"Image type {imageType} is not supported for SVG recoding.")
        };
    }
}
