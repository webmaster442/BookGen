using Bookgen.Lib.Domain.IO.Configuration;

using SkiaSharp;

namespace Bookgen.Lib.ImageService;

public static class ImageConverter
{
    public static void Encode(string source, string output, ImageType imageType, int width, int height, int quality)
    {
        using var srcStream= File.OpenRead(source);
        using var destStream = File.Create(output);
        
        if (Path.GetExtension("soruce").Equals(".svg", StringComparison.OrdinalIgnoreCase))
        {
            SKData img = Utils.RenderSvg(srcStream, width, height, GetRecodeOption(imageType));
            img.SaveTo(destStream);
        }

        using SKBitmap loaded = SKBitmap.Decode(srcStream);
        using SKBitmap result = Utils.ResizeIfBigger(loaded, width, height);

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
