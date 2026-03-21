//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

using BookGen.Vfs;

namespace Bookgen.Lib.Markdown.Renderers.Embedders;

internal abstract class BaseLinkEmbedder
{
    protected static bool IsHost(Uri input, params string[] hosts)
    {
        foreach (var host in hosts)
        {
            if (input.Host.EndsWith(host))
            {
                return true;
            }
        }
        return false;
    }

    protected static bool TryGetQueryParam(Uri uri,
                                           string queryparam,
                                           [NotNullWhen(true)] out string? value)
    {
        var query = uri.Query;
        if (string.IsNullOrEmpty(query))
        {
            value = null;
            return false;
        }
        NameValueCollection queryParams = System.Web.HttpUtility.ParseQueryString(query);

        value = queryParams[queryparam];
        return value != null;
    }

    protected bool TryDownloadAndBase64Encode(Uri url, out string? result)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:140.0) Gecko/20100101 Firefox/140.0");

        using HttpResponseMessage? response = client.GetAsync(url).GetAwaiter().GetResult();

        if (response.IsSuccessStatusCode)
        {
            using var stream = response.Content.ReadAsStream();

            result = stream.Base64Encode();
            return true;

        }

        result = string.Empty;
        return false;
    }

    public abstract bool TryRender(Uri input, [NotNullWhen(true)] out string? rendered);
}
