//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using SkiaSharp;
using Svg.Skia;
using System;

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

        private static (int renderWidth, int renderHeight, float scale) CalcNewSize(SKRect size, 
                                                                                    int maxwidth, 
                                                                                    int maxHeight)
        {
            float scale = 1.0f;
            float renderWidth = maxwidth;
            float renderHeight = maxHeight;

            if (size.Width > maxwidth || size.Height > maxHeight)
            {
                float imgMax = Math.Max(size.Width, size.Height);
                float targetMin = Math.Min(maxwidth, maxHeight);
                scale = targetMin / imgMax;
            }

            return (renderWidth: (int)(renderWidth * scale), 
                    renderHeight: (int)(renderHeight * scale), 
                    scale);

        }

        public static SKData SvgToPng(FsPath svgFile, int maxWidht, int maxHeight)
        {
            var svg = new SKSvg();
            svg.Load(svgFile.ToString());

            if (svg.Picture == null)
                return SKData.Empty;

            SKRect svgSize = svg.Picture.CullRect;

            (int renderWidth, int renderHeight, float scale) sizeData = CalcNewSize(svgSize, maxWidht, maxHeight);

            var matrix = SKMatrix.MakeScale(sizeData.scale, sizeData.scale);

            using (SKBitmap bitmap = new SKBitmap(sizeData.renderWidth, sizeData.renderHeight))
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

            (int renderWidth, int renderHeight, float scale) sizeData = CalcNewSize(new SKRect(0, 0, input.Width, input.Height), width, height);


            return input.Resize(new SKImageInfo(sizeData.renderWidth, sizeData.renderHeight), SKFilterQuality.High);
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
