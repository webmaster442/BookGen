//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using SkiaSharp;
using System;
using SKSvg = SkiaSharp.Extended.Svg.SKSvg;

namespace BookGen.Utilities
{
    internal static class ImageUtils
    {
        public static bool IsImage(FsPath file)
        {
            switch (file.Extension)
            {
                case ".png":
                case ".jpg":
                case ".jpeg":
                case ".svg":
                case ".gif":
                case ".webp":
                    return true;
                default:
                    return false;
            }
        }

        public static SKEncodedImageFormat GetSkiaImageFormat(FsPath file)
        {
            switch (file.Extension)
            {
                case ".png":
                    return SKEncodedImageFormat.Png;
                case ".jpg":
                case ".jpeg":
                    return SKEncodedImageFormat.Jpeg;
                case ".gif":
                    return SKEncodedImageFormat.Gif;
                case ".webp":
                    return SKEncodedImageFormat.Webp;
                default:
                    throw new InvalidOperationException("Unknown file type");
            }
        }

        public static bool IsSvg(FsPath file)
        {
            return file.Extension == ".svg";
        }

        public static SKData SvgToPng(FsPath svgFile, int width, int height)
        {
            var svg = new SKSvg();
            svg.Load(svgFile.ToString());

            SKRect svgSize = svg.Picture.CullRect;

            float scale = 1.0f;
            float renderWidth = svgSize.Width;
            float renderHeight = svgSize.Height;

            if (svgSize.Width > width || svgSize.Height > height)
            {
                float svgMax = Math.Max(svgSize.Width, svgSize.Height);
                float canvasMin = Math.Min(width, height);
                scale = canvasMin / svgMax;
                renderWidth = svgSize.Width * scale;
                renderHeight = svgSize.Height * scale;
            }

            var matrix = SKMatrix.MakeScale(scale, scale);

            using (SKBitmap bitmap = new SKBitmap((int)renderWidth, (int)renderHeight))
            {
                using (SKCanvas canvas = new SKCanvas(bitmap))
                {
                    canvas.DrawPicture(svg.Picture, ref matrix);
                    canvas.Flush();
                }

                using (SKImage image = SKImage.FromBitmap(bitmap))
                {
                    return image.Encode(SKEncodedImageFormat.Png, 100);
                }
            }
        }

        public static SKBitmap LoadImage(FsPath file)
        {
            return SKBitmap.Decode(file.ToString());
        }

        public static SKBitmap ResizeIfBigger(SKBitmap input, int width, int height)
        {
            if (input.Width < width && input.Height < height)
                return input;

            return input.Resize(new SKImageInfo(width, height), SKFilterQuality.High);
        }

        public static SKData EncodeToFormat(SKBitmap bitmap, SKEncodedImageFormat format)
        {
            using (SKImage image = SKImage.FromBitmap(bitmap))
            {
                return image.Encode(format, 100);
            }
        }

        public static SKData EncodeWebp(SKBitmap bitmap, int quality)
        {
            using (SKImage image = SKImage.FromBitmap(bitmap))
            {
                return image.Encode(SKEncodedImageFormat.Webp, quality);
            }
        }
    }
}
