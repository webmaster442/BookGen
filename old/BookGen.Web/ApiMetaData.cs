//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace BookGen.Web;

internal sealed record class ApiMetaData : UrlMetaData
{
    [SetsRequiredMembers]
    public ApiMetaData(string path, string mime, ApiMethod method = ApiMethod.Get) : base(path, mime)
    {
        Method = method;
    }

    public required ApiMethod Method { get; init; }


}
