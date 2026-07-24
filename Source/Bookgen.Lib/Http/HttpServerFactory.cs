//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Net.Mime;
using System.Net.NetworkInformation;

using BookGen.Lib.AppSettings;
using BookGen.Vfs;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BookGen.Lib.Http;

public static class HttpServerFactory
{
    public const int HostingPort = 8081;
    public const int PreviewPort = 8181;

    private static int ChoosePort(int @default = HostingPort)
    {
        IPGlobalProperties ipProps = IPGlobalProperties.GetIPGlobalProperties();

        TcpConnectionInformation[] props = ipProps.GetActiveTcpConnections();

        IEnumerable<int> tcpConnections = ipProps.GetActiveTcpConnections()
            .Where(c => c.State == TcpState.Listen)
            .Select(c => c.LocalEndPoint.Port);

        IEnumerable<int> tcpListeners = ipProps.GetActiveTcpListeners()
            .Select(l => l.Port);

        HashSet<int> usedPorts = tcpConnections.Concat(tcpListeners).ToHashSet();

        if (usedPorts.Contains(@default))
        {
            int min = @default + 1;
            int max = @default + 100;

            for (int i = min; i < max; i++)
            {
                if (!usedPorts.Contains(i))
                {
                    return i;
                }
            }

            throw new InvalidOperationException($"No available port found in the range {min} and {max}");
        }

        return @default;

    }

    public static IHttpServer CreateServerForDirectoryHosting(string directoryToServe, ILogger logger)
    {
        var server = new HttpServer(ChoosePort(), logger);
        server.AddStaticFiles(directory: directoryToServe, requestPath: "", directoryBrowseEnabled: true);
        server.AddRoute(new ApiMetaData("/qrcodelink", MediaTypeNames.Text.Html), async context =>
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = MediaTypeNames.Text.Html;
            await context.Response.WriteAsync(PageFactory.GetQrCodePage(server.GetListenUrls()));
        });
        return server;
    }

    public static IHttpServer CreateServerForPreview(IReadOnlyFileSystem source,
                                                     ILogger logger,
                                                     IProgramPathResolver programPathResolver,
                                                     IAssetSource assetSource)
    {
        var server = new HttpServer(ChoosePort(), logger);
        var previewRoutes = new PreviewRoutes(source, logger, programPathResolver, assetSource);
        server.AddRoutes(previewRoutes);
        return server;
    }
}
