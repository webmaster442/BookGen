using System.Diagnostics;

using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.VFS;

using SkiaSharp;

namespace Bookgen.Lib.ImageService;
public sealed class ImgService : IImgService
{
    private readonly IFolder _sourceFolder;
    private readonly ImageConfig _imageConfig;

    public ImgService(IFolder sourceFolder, ImageConfig imageConfig)
    {
        _sourceFolder = sourceFolder;
        _imageConfig = imageConfig;
    }

    private static bool IsImage(string file)
    {
        return Path.GetExtension(file).ToLower() switch
        {
            ".png" or ".jpg" or ".jpeg" or ".svg" or ".gif" or ".webp" => true,
            _ => false,
        };
    }

    private static ImageType GetImageType(string file)
    {
        return Path.GetExtension(file).ToLower() switch
        {
            ".png" => ImageType.Png,
            ".jpg" or ".jpeg" => ImageType.Jpeg,
            ".svg" => ImageType.Svg,
            ".gif" => ImageType.Gif,
            ".webp" => ImageType.Webp,
            _ => throw new UnreachableException(),
        };
    }

    public (string data, ImageType imageType) GetImageEmbedData(string path)
    {
        static ImageType GetImateType(SvgRecodeOption recodeOption)
        {
            return recodeOption switch
            {
                SvgRecodeOption.AsPng => ImageType.Png,
                SvgRecodeOption.AsWebp => ImageType.Webp,
                SvgRecodeOption.Passtrough => ImageType.Svg,
                _ => throw new UnreachableException(),
            };
        }

        if (!IsImage(path))
            throw new InvalidOperationException($"{path} is not an image");

        using Stream fileData = _sourceFolder.OpenStream(path);

        if (string.Equals(Path.GetExtension(path), ".svg", StringComparison.CurrentCultureIgnoreCase))
        {
            if (_imageConfig.SvgRecode == SvgRecodeOption.Passtrough)
                return (_sourceFolder.ReadText(path), ImageType.Svg);

            using SKData rendered = Utils.RenderSvg(fileData,
                                                    _imageConfig.ResizeWith,
                                                    _imageConfig.ResizeHeight,
                                                    _imageConfig.SvgRecode);

            return (Convert.ToBase64String(rendered.AsSpan()), GetImateType(_imageConfig.SvgRecode));

        }

        if (_imageConfig.ResizeAndRecodeImagesToWebp)
        {
            using SKData rendered = Utils.EncodeToWebp(fileData,
                                                       _imageConfig.ResizeWith,
                                                       _imageConfig.ResizeHeight,
                                                       _imageConfig.WebpQuality);

            return (Convert.ToBase64String(rendered.AsSpan()), ImageType.Webp);
        }

        return (Utils.Base64Enode(fileData), GetImageType(path));
    }
}
