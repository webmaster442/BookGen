//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


using System.Diagnostics.CodeAnalysis;

namespace Bookgen.Lib.ImageService;

public sealed record class ImageResult
{
    public required ImageType ImageType { get; set; }
    public required string Data { get; set; }
    public required string OriginalName { get; set; }

    public static bool TryParse(string imgUrl, [NotNullWhen(true)] out ImageResult? result)
    {
        if (string.IsNullOrWhiteSpace(imgUrl) || !imgUrl.StartsWith("data:"))
        {
            result = null!;
            return false;
        }

        string[] parts = imgUrl.Split([','], 2);

        switch (parts[0])
        {
            case "data:image/jpeg;base64":
                result = new ImageResult
                {
                    ImageType = ImageType.Jpeg,
                    Data = parts[1],
                    OriginalName = $"image-{Guid.CreateVersion7()}.jpg"
                };
                return true;
            case "data:image/png;base64":
                result = new ImageResult
                {
                    ImageType = ImageType.Png,
                    Data = parts[1],
                    OriginalName = $"image-{Guid.CreateVersion7()}.png"
                };
                return true;
            case "data:image/webp;base64":
                result = new ImageResult
                {
                    ImageType = ImageType.Webp,
                    Data = parts[1],
                    OriginalName = $"image-{Guid.CreateVersion7()}.webp"
                };
                return true;
            default:
                result = null!;
                return false;
        }
    }
}
