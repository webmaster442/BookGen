//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using Bookgen.Lib.Domain.IO.Configuration;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;

using SkiaSharp;

namespace Bookgen.Lib.ImageService;

public sealed class ImgService : IImgService
{
    private readonly IReadOnlyFileSystem _sourceFolder;
    private readonly ImageConfig _imageConfig;
    private readonly ILogger _logger;

    public ImgService(IReadOnlyFileSystem sourceFolder, ILogger logger, ImageConfig imageConfig)
    {
        _sourceFolder = sourceFolder;
        _imageConfig = imageConfig;
        _logger = logger;
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

    public ImageResult GetImageEmbedData(string path)
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

        path = path.Replace("../", "");

        if (!IsImage(path))
            throw new InvalidOperationException($"{path} is not an image");

        if (!_sourceFolder.FileExists(path))
        {
            _logger.LogWarning("Image {Path} does not exist in source folder", path);
            return new ImageResult
            {
                Data = string.Empty,
                ImageType = ImageType.Png,
                OriginalName = path,
            };
        }

        using Stream fileData = _sourceFolder.OpenReadStream(path);

        if (string.Equals(Path.GetExtension(path), ".svg", StringComparison.CurrentCultureIgnoreCase))
        {
            if (_imageConfig.SvgRecode == SvgRecodeOption.Passtrough)
            {
                return new ImageResult
                {
                    Data = _sourceFolder.ReadAllText(path),
                    ImageType = ImageType.Svg,
                    OriginalName = path,
                };
            }

            using SKData rendered = Utils.RenderSvg(fileData,
                                                    _imageConfig.ResizeWith,
                                                    _imageConfig.ResizeHeight,
                                                    _imageConfig.SvgRecode);

            return new ImageResult
            {
                Data = Convert.ToBase64String(rendered.AsSpan()),
                ImageType = GetImateType(_imageConfig.SvgRecode),
                OriginalName = path,
            };

        }

        if (_imageConfig.ResizeAndRecodeImages != ImgRecodeOption.Passtrough)
        {
            using SKData rendered = Utils.Encode(fileData,
                                                 _imageConfig.ResizeWith,
                                                 _imageConfig.ResizeHeight,
                                                 _imageConfig.ImageQualityOnResize,
                                                 _imageConfig.ResizeAndRecodeImages);

            var type = _imageConfig.ResizeAndRecodeImages switch
            {
                ImgRecodeOption.AsPng => ImageType.Png,
                ImgRecodeOption.AsWebp => ImageType.Webp,
                _ => throw new UnreachableException(),
            };

            return new ImageResult
            {
                Data = Convert.ToBase64String(rendered.AsSpan()),
                ImageType = type,
                OriginalName = path,
            };

        }

        return new ImageResult
        {
            Data = Utils.Base64Encode(fileData),
            ImageType = GetImageType(path),
            OriginalName = path,
        };
    }
}
