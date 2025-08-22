//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace Bookgen.Lib.Http;

internal record class UrlMetaData
{
    public required string Path { get; init; }
    public required string MimeType { get; init; }

    [SetsRequiredMembers]
    public UrlMetaData(string path, string mime)
    {
        Path = path;
        MimeType = mime;
    }
}
