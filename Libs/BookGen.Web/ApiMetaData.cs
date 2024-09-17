//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Web;

internal sealed record class ApiMetaData : UrlMetaData
{
    public required ApiMethod Method { get; init; }
}
