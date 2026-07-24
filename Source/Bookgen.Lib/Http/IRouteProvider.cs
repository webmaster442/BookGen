using Microsoft.AspNetCore.Http;

namespace BookGen.Lib.Http;

internal interface IRouteProvider
{
    IEnumerable<(ApiMetaData metaData, RequestDelegate handler)> Routes { get; }
}
