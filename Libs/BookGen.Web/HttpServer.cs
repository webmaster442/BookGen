using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace BookGen.Web;

internal sealed class HttpServer : IHttpServer
{
    private readonly WebApplication _app;

    public int Port { get; }

    public HttpServer(int port)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        builder.Logging.ClearProviders();
        builder.Logging.SetMinimumLevel(LogLevel.Error);
        builder.Logging.AddConsole();
        builder.WebHost.ConfigureKestrel((context, serverOptions) => serverOptions.ListenAnyIP(port));
        _app = builder.Build();
        Port = port;

        _app.UseStatusCodePages(async statusCodeContext =>
        {
            var content = ErrorPageFactory.GetErrorPage(statusCodeContext.HttpContext.Response.StatusCode, "");
            statusCodeContext.HttpContext.Response.ContentType = "text/html";
            await statusCodeContext.HttpContext.Response.WriteAsync(content);
        });

        _app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                context.Response.StatusCode = 500;
                var content = ErrorPageFactory.GetErrorPage(context.Response.StatusCode, "Internal server error");
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(content);
            });
        });
    }

    public void AddStaticFiles(string directory, string requestPath)
    {
        _app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(directory),
            RequestPath = requestPath
        });
    }

    public void AddMemoryFiles(IReadOnlyDictionary<UrlMetaData, byte[]> files)
    {
        foreach (var file in files)
        {
            _app.MapGet(file.Key.Path, async context =>
            {
                context.Response.ContentType = file.Key.MimeType;
                context.Response.ContentLength = file.Value.Length;
                context.Response.StatusCode = StatusCodes.Status200OK;
                await context.Response.Body.WriteAsync(file.Value);
            });
        }
    }

    public void AddRoutes(IReadOnlyDictionary<ApiMetaData, RequestDelegate> routes)
    {
        foreach (var route in routes)
        {
            var meta = route.Key;
            var handler = route.Value;

            switch (meta.Method)
            {
                case ApiMethod.Get:
                    _app.MapGet(meta.Path, async context => await handler(context));
                    break;
                case ApiMethod.Post:
                    _app.MapPost(meta.Path, async context => await handler(context));
                    break;
                case ApiMethod.Put:
                    _app.MapPut(meta.Path, async context => await handler(context));
                    break;
                case ApiMethod.Delete:
                    _app.MapDelete(meta.Path, async context => await handler(context));
                    break;
                case ApiMethod.Patch:
                    _app.MapPatch(meta.Path, async context => await handler(context));
                    break;
                case ApiMethod.Head:
                case ApiMethod.Options:
                case ApiMethod.Trace:
                    _app.Map(meta.Path, async context => await handler(context));
                    break;
            }
        }
    }

    public ICollection<string> GetListenUrls()
    {
        return _app.Urls;
    }

    public static IEnumerable<(IPAddress adress, IPAddress mask)> GetIpAdresses()
    {
        var ipAdresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList
            .Where(i => i.AddressFamily == AddressFamily.InterNetwork)
            .ToHashSet();

        var ifaceAddrs = NetworkInterface.GetAllNetworkInterfaces()
            .Where(i => i.OperationalStatus == OperationalStatus.Up)
            .SelectMany(x => x.GetIPProperties().UnicastAddresses)
            .Where(x => ipAdresses.Contains(x.Address));

        foreach (var adress in ifaceAddrs)
        {
            yield return (adress.Address, adress.IPv4Mask);
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
        => await _app.StartAsync(cancellationToken);
}
