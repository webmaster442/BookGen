//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Security.Cryptography;
using System.Text;

using Bookgen.Lib.ImageService;

namespace Bookgen.Lib.Internals;

internal static class IdGenerator
{
    public static string GenerateImageFileName(string orignalPath, ImageType imageType)
    {
        var extension = imageType switch
        {
            ImageType.Jpeg => "jpg",
            ImageType.Png => "png",
            ImageType.Gif => "gif",
            ImageType.Webp => "webp",
            ImageType.Svg => "svg",
            _ => throw new ArgumentOutOfRangeException(nameof(imageType), imageType, null)
        };

        return $"{Generate32BitDeterministicId(orignalPath)}.{extension}";
    }

    public static string Generate32BitDeterministicId(string input)
    {
        const uint prime = 0x01000193;
        uint hash = 0x811c9dc5;
        foreach (var chr in input)
        {
            hash = (hash ^ chr) * prime;
        }
        return Convert.ToHexString(BitConverter.GetBytes(hash));
    }

    public static string Generate128BitId(string input)
    {
        var hashBytes = SHA1.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(hashBytes);
    }
}
