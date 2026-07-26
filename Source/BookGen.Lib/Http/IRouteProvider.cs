//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Microsoft.AspNetCore.Http;

namespace BookGen.Lib.Http;

internal interface IRouteProvider
{
    IEnumerable<(ApiMetaData metaData, RequestDelegate handler)> Routes { get; }
}
