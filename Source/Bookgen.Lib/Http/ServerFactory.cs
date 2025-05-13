//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Net.Mime;

using Microsoft.AspNetCore.Http;

namespace Bookgen.Lib.Http;
public static class ServerFactory
{
    private const int HostingPort = 8081;
    private const int TestingPort = 8090;

    public static IHttpServer CreateServerForDirectoryHosting(string directoryToServe)
    {
        var server = new HttpServer(HostingPort);
        server.AddStaticFiles(directoryToServe, "", true);
        server.AddRoute(new ApiMetaData("/qrcodelink", MediaTypeNames.Text.Html), async context =>
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = MediaTypeNames.Text.Html;
            await context.Response.WriteAsync(PageFactory.GetQrCodePage(server.GetListenUrls()));
        });
        return server;
    }

    public static IHttpServer CreateServerForTesting(string directoryToServe)
    {
        var server = new HttpServer(TestingPort);
        server.AddStaticFiles(directoryToServe, "", false);
        server.AddRoute(new ApiMetaData("/qrcodelink", MediaTypeNames.Text.Html), async context =>
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = MediaTypeNames.Text.Html;
            await context.Response.WriteAsync(PageFactory.GetQrCodePage(server.GetListenUrls()));
        });
        return server;
    }
}
