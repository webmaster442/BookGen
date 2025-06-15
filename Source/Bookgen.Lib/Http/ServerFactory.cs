//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Net.Mime;
using System.Net.NetworkInformation;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Http;
public static class ServerFactory
{
    public const int HostingPort = 8081;

    private static int ChoosePort(int @default = HostingPort)
    {
        IPGlobalProperties ipProps = IPGlobalProperties.GetIPGlobalProperties();

        var props = ipProps.GetActiveTcpConnections();

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
}
