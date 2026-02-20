//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Buffers;
using System.Diagnostics;
using System.Text;

using Bookgen.Lib.Domain.IO.Configuration;

using SkiaSharp;

using Svg.Skia;

namespace Bookgen.Lib.ImageService;

internal static class Utils
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

    public static SKData RenderSvg(Stream stream, int maxWidth, int maxHeight, SvgRecodeOption svgRecode)
    {
        using var svg = new SKSvg();
        svg.Load(stream);

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


    public static string Base64Encode(Stream fileData)
    {
        const int readBufferSize = 4096;

        int size = (int)Math.Ceiling(4 * (fileData.Length / 3.0));

        var stringBuilder = new StringBuilder(size);

        byte[] remainder = new byte[2];
        int remainderCount = 0;

        byte[] readBuffer = ArrayPool<byte>.Shared.Rent(readBufferSize);
        int bytesRead;

        try
        {
            while ((bytesRead = fileData.Read(readBuffer, 0, readBuffer.Length)) > 0)
            {
                int totalBytesToProcess = remainderCount + bytesRead;

                // Determine how many bytes can form complete 3-byte triplets.
                var bytesToTakeForEncoding = (totalBytesToProcess / 3) * 3;

                if (bytesToTakeForEncoding > 0)
                {
                    var chunk = ArrayPool<byte>.Shared.Rent(bytesToTakeForEncoding);
                    remainder.AsSpan(0, remainderCount).CopyTo(chunk);
                    readBuffer.AsSpan(0, bytesToTakeForEncoding - remainderCount).CopyTo(chunk.AsSpan(remainderCount));
                    stringBuilder.Append(Convert.ToBase64String(chunk, 0, bytesToTakeForEncoding));
                    ArrayPool<byte>.Shared.Return(chunk);

                    remainderCount = bytesRead - (bytesToTakeForEncoding - remainderCount);
                    readBuffer.AsSpan(bytesRead - remainderCount, remainderCount).CopyTo(remainder);
                }
                else
                {
                    readBuffer.AsSpan(0, bytesRead).CopyTo(remainder.AsSpan(remainderCount));
                    remainderCount += bytesRead;
                }
            }

            if (remainderCount > 0)
            {
                stringBuilder.Append(Convert.ToBase64String(remainder.AsSpan(0, remainderCount).ToArray()));
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(readBuffer);
        }

        return stringBuilder.ToString();
    }
}
