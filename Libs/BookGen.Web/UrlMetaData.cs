//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Web;

internal record class UrlMetaData
{
    public required string Path { get; init; }
    public required string MimeType { get; init; }
}
