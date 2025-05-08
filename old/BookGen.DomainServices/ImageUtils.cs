//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;

using Microsoft.Extensions.Logging;

using SkiaSharp;

using Svg.Skia;

namespace BookGen.DomainServices
{
    public static class ImageUtils
    {
        public static bool IsImage(FsPath file)
        {
            return file.Extension switch
            {
                ".png" or ".jpg" or ".jpeg" or ".svg" or ".gif" or ".webp" => true,
                _ => false,
            };
        }

        public static SKEncodedImageFormat GetSkiaImageFormat(string extension)
        {
            return extension.ToLower() switch
            {
                ".png" or "png" => SKEncodedImageFormat.Png,
                ".jpg" or "jpg" or ".jpeg" or "jpeg" => SKEncodedImageFormat.Jpeg,
                ".gif" or "gif" => SKEncodedImageFormat.Gif,
                ".webp" or "webp" => SKEncodedImageFormat.Webp,
                _ => throw new InvalidOperationException("Unknown file type"),
            };
        }

        public static SKEncodedImageFormat GetSkiaImageFormat(FsPath file)
        {
            return GetSkiaImageFormat(file.Extension);
        }

        public static bool IsSvg(FsPath file)
        {
            return file.Extension == ".svg";
        }

        private static (int renderWidth, int renderHeight, float scale) CalcNewSize(SKRect size,
                                                                                    int maxwidth,
                                                                                    int maxHeight)
        {
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

        public static SKData EncodeSvg(FsPath svgFile, int maxWidht, int maxHeight, SKEncodedImageFormat format = SKEncodedImageFormat.Png)
        {
            var svg = new SKSvg();
            svg.Load(svgFile.ToString());

            if (svg.Picture == null)
                return SKData.Empty;

            SKRect svgSize = svg.Picture.CullRect;

            (int renderWidth, int renderHeight, float scale) = CalcNewSize(svgSize, maxWidht, maxHeight);

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
                    return image.Encode(format, 100);
                }
            }
        }

        public static SKBitmap LoadImage(FsPath file)
        {
            return SKBitmap.Decode(file.ToString());
        }

        public static SKBitmap ResizeIfBigger(SKBitmap input, int? width, int? height)
        {
            if (width == null && height == null)
                return input;

            int w = width ?? input.Width;
            int h = height ?? input.Height;

            if (input.Width < w && input.Height < h)
                return input;

            (int renderWidth, int renderHeight, float scale) = CalcNewSize(new SKRect(0, 0, input.Width, input.Height), w, h);

            return input.Resize(new SKImageInfo(renderWidth, renderHeight), SKSamplingOptions.Default);
        }

        public static SKData EncodeToFormat(SKBitmap bitmap, SKEncodedImageFormat format, int quality = 100)
        {
            using var image = SKImage.FromBitmap(bitmap);
            return image.Encode(format, quality);
        }

        public static SKBitmap LoadForConvert(FsPath input, int? maxWidth, int? maxHeight)
        {
            if (IsSvg(input))
            {
                int targetWidth = maxWidth ?? int.MaxValue;
                int taregetHeight = maxHeight ?? int.MaxValue;

                var svg = new SKSvg();
                svg.Load(input.ToString());

                if (svg.Picture == null)
                    return new SKBitmap(1, 1);

                SKRect svgSize = svg.Picture.CullRect;

                (int renderWidth, int renderHeight, float scale) = CalcNewSize(svgSize, targetWidth, taregetHeight);

                var matrix = SKMatrix.CreateScale(scale, scale);

                var bitmap = new SKBitmap(renderWidth, renderHeight);
                using (var canvas = new SKCanvas(bitmap))
                {
                    canvas.DrawPicture(svg.Picture, matrix);
                    canvas.Flush();
                }

                return bitmap;
            }

            return LoadImage(input);
        }

        public static bool ConvertImageFile(ILogger log, FsPath input, FsPath output, int quality, int? width, int? height, string? format = null)
        {
            SKEncodedImageFormat targetFormat;
            using (SKBitmap image = LoadForConvert(input, width, height))
            {
                if (!string.IsNullOrEmpty(format))
                {
                    targetFormat = GetSkiaImageFormat(format);
                }
                else
                {
                    targetFormat = GetSkiaImageFormat(output);
                }
                using (SKBitmap resized = ResizeIfBigger(image, width, height))
                {
                    using SKData encoded = EncodeToFormat(resized, targetFormat, quality);
                    using (FileStream? stream = output.CreateStream(log))
                    {
                        try
                        {
                            encoded.SaveTo(stream);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            log.LogWarning(ex, "ConvertImageFile failed");
                            return false;
                        }
                    }
                }
            }
        }
    }
}
