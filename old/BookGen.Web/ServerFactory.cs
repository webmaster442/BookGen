//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Net.Mime;

using BookGen.Resources;

using Microsoft.AspNetCore.Http;

namespace BookGen.Web;
public static class ServerFactory
{
    private const int HostingPort = 8081;
    private const int TestingPort = 8090;

    public static IHttpServer CreateServerForDirectoryHosting(string directoryToServe)
    {
        var server = new HttpServer(HostingPort);
        server.AddStaticFiles(directoryToServe, "", true);
        server.AddMemoryFile(new UrlMetaData("/favicon.ico", MediaTypeNames.Image.Icon), StreamToByteArray(ResourceHandler.GetFileStream(KnownFile.FaviconFs)));
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

    private static byte[] StreamToByteArray(Stream input)
    {
        if (input.CanSeek)
        {
            long length = input.Length;
            byte[] buffer = new byte[length];
            int bytesRead = 0;
            while (bytesRead < length)
            {
                int read = input.Read(buffer, bytesRead, (int)(length - bytesRead));
                if (read == 0) break;
                bytesRead += read;
            }
            return buffer;
        }

        throw new InvalidOperationException("Stream does not support seeking, cannot pre-allocate buffer.");
    }
}
