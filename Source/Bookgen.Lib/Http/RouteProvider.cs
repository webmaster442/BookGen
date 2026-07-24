using Microsoft.AspNetCore.Http;

namespace BookGen.Lib.Http;

internal interface IRouteProvider
{
    IEnumerable<KeyValuePair<ApiMetaData, RequestDelegate>> Routes { get; }
}
