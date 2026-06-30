//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Net.Mime;

namespace Bookgen.Lib.Rendering.Images;

public static class ImageTypeExtensions
{
    public static string GetMimeType(this ImageType type)
    {
        return type switch
        {
            ImageType.Png => MediaTypeNames.Image.Png,
            ImageType.Webp => MediaTypeNames.Image.Webp,
            ImageType.Jpeg => MediaTypeNames.Image.Jpeg,
            ImageType.Svg => MediaTypeNames.Image.Svg,
            ImageType.Gif => MediaTypeNames.Image.Gif,
            _ => throw new UnreachableException(),
        };
    }
}
