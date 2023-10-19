//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using SkiaSharp;
using System.Text;

namespace BookGen.Gui;

internal static class TextHelper
{
    public static IReadOnlyList<string> GetLines(string text, int lineMaxWidth)
    {
        var buffer = new List<string>(8);
        using (var reader = new StringReader(text))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length < lineMaxWidth)
                {
                    buffer.Add(line);
                }
                else
                {
                    string[] newLines = line
                        .Chunk(lineMaxWidth)
                        .Select(chrArray => new string(chrArray))
                        .ToArray();

                    buffer.AddRange(newLines);
                }
            }
        }
        return buffer;
    }

    public static string ImageStreamToMarkup(Stream source, int targetWidth, int targetHeight)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(targetWidth, 1);

        ArgumentOutOfRangeException.ThrowIfLessThan(targetHeight, 1);

        using SKBitmap image = SKBitmap.Decode(source);
        using SKBitmap resized = Resize(image, targetWidth, targetHeight);

        return Encode(resized);
    }

    private static SKBitmap Resize(SKBitmap image, int targetWidth, int targetHeight)
    {
        if (image.Width < targetWidth && image.Height < targetHeight)
            return image;

        (int renderWidth, int renderHeight) = CalcNewSize(new SKRect(0, 0, image.Width, image.Height),
                                                          targetWidth,
                                                          targetHeight);

        return image.Resize(new SKImageInfo(renderWidth, renderHeight), SKFilterQuality.High);
    }

    private static (int renderWidth, int renderHeight) CalcNewSize(SKRect inputSize, int maxwidth, int maxHeight)
    {
        float scale = 1.0f;

        if (inputSize.Width > maxwidth || inputSize.Height > maxHeight)
        {
            scale = maxwidth / inputSize.Width;
        }

        return (renderWidth: (int)(inputSize.Width * scale),
                renderHeight: maxHeight);
    }

    private static string Encode(SKBitmap finalSize)
    {
        StringBuilder buffer = new(finalSize.Width);
        for (int y = 0; y < finalSize.Height; y += 2)
        {
            for (int x = 0; x < finalSize.Width; x++)
            {
                var foregroundColor = finalSize.GetPixel(x, y);
                var backgroundColor = finalSize.GetPixel(x, y + 1);
                buffer.AppendFormat("\x1b[48;2;{0};{1};{2}m\x1b[38;2;{3};{4};{5}m▀",
                                    backgroundColor.Red,
                                    backgroundColor.Green,
                                    backgroundColor.Blue,
                                    foregroundColor.Red,
                                    foregroundColor.Green,
                                    foregroundColor.Blue);
            }
            buffer.AppendLine();
        }
        return buffer.ToString();
    }
}
