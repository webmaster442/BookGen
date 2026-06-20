//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using Bookgen.Lib.Domain.IO.Configuration;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;

using SkiaSharp;

namespace Bookgen.Lib.Rendering.Images;

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

    public ImageResult GetImageEmbedData(string filePath)
    {
        filePath = filePath.Replace("../", "");

        if (!IsImage(filePath))
            throw new InvalidOperationException($"{filePath} is not an image");

        if (!_sourceFolder.FileExists(filePath))
        {
            _logger.LogWarning("Image {Path} does not exist in source folder", filePath);
            return new ImageResult
            {
                Data = string.Empty,
                ImageType = ImageType.Png,
                OriginalName = filePath,
            };
        }

        using Stream fileData = _sourceFolder.OpenReadStream(filePath);

        if (string.Equals(Path.GetExtension(filePath), ".svg", StringComparison.CurrentCultureIgnoreCase))
        {
            if (_imageConfig.SvgRecode == SvgRecodeOption.Passtrough)
            {
                return new ImageResult
                {
                    Data = _sourceFolder.ReadAllText(filePath),
                    ImageType = ImageType.Svg,
                    OriginalName = filePath,
                };
            }

            using SKData rendered = ImageUtils.RenderSvg(fileData,
                                                    _imageConfig.ResizeWith,
                                                    _imageConfig.ResizeHeight,
                                                    _imageConfig.SvgRecode);

            return new ImageResult
            {
                Data = Convert.ToBase64String(rendered.AsSpan()),
                ImageType = GetImateType(_imageConfig.SvgRecode),
                OriginalName = filePath,
            };

        }

        if (_imageConfig.ResizeAndRecodeImages != ImgRecodeOption.Passtrough)
        {
            using SKData rendered = ImageUtils.Encode(fileData,
                                                 _imageConfig.ResizeWith,
                                                 _imageConfig.ResizeHeight,
                                                 _imageConfig.ImageQualityOnResize,
                                                 _imageConfig.ResizeAndRecodeImages);

            ImageType type = _imageConfig.ResizeAndRecodeImages switch
            {
                ImgRecodeOption.AsPng => ImageType.Png,
                ImgRecodeOption.AsWebp => ImageType.Webp,
                _ => throw new UnreachableException(),
            };

            return new ImageResult
            {
                Data = Convert.ToBase64String(rendered.AsSpan()),
                ImageType = type,
                OriginalName = filePath,
            };

        }

        return new ImageResult
        {
            Data = fileData.Base64Encode(),
            ImageType = GetImageType(filePath),
            OriginalName = filePath,
        };
    }
}
