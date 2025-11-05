//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace Bookgen.Lib.ImageService;

public sealed record class ImageResult
{
    public required ImageType ImageType { get; set; }
    public required string Data { get; set; }
    public required string OriginalName { get; set; }
}
