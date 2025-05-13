//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Http;

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

        var options = new DefaultFilesOptions();
        options.DefaultFileNames.Add("index.xml");
        options.DefaultFileNames.Add("index.json");
        options.DefaultFileNames.Add("index.txt");
        options.DefaultFileNames.Add("default.xml");
        options.DefaultFileNames.Add("default.json");
        options.DefaultFileNames.Add("default.txt");
        _app.UseDefaultFiles(options);

        _app.UseStatusCodePages(async statusCodeContext =>
        {
            var content = PageFactory.GetErrorPage(statusCodeContext.HttpContext.Response.StatusCode, "");
            statusCodeContext.HttpContext.Response.ContentType = "text/html";
            await statusCodeContext.HttpContext.Response.WriteAsync(content);
        });

        _app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                context.Response.StatusCode = 500;
                var content = PageFactory.GetErrorPage(context.Response.StatusCode, "Internal server error");
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(content);
            });
        });
    }

    public void AddStaticFiles(string directory, string requestPath, bool directoryBrowseEnabled)
    {
        var options = new FileServerOptions
        {
            FileProvider = new PhysicalFileProvider(directory),
            RequestPath = requestPath,
            EnableDefaultFiles = true,
            EnableDirectoryBrowsing = directoryBrowseEnabled,
        };
        options.StaticFileOptions.ContentTypeProvider = new FileExtensionContentTypeProvider();
        options.DefaultFilesOptions.DefaultFileNames.Add("index.xml");
        options.DefaultFilesOptions.DefaultFileNames.Add("index.json");
        options.DefaultFilesOptions.DefaultFileNames.Add("index.txt");
        options.DefaultFilesOptions.DefaultFileNames.Add("default.xml");
        options.DefaultFilesOptions.DefaultFileNames.Add("default.json");
        options.DefaultFilesOptions.DefaultFileNames.Add("default.txt");

        _app.UseFileServer(options);
    }

    public void AddMemoryFiles(IReadOnlyDictionary<UrlMetaData, byte[]> files)
    {
        foreach (var file in files)
        {
            AddMemoryFile(file.Key, file.Value);
        }
    }

    public void AddMemoryFile(UrlMetaData metaData, byte[] content)
    {
        _app.MapGet(metaData.Path, async context =>
        {
            context.Response.ContentType = metaData.MimeType;
            context.Response.ContentLength = content.Length;
            context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.Body.WriteAsync(content);
        });
    }

    public void AddRoute(ApiMetaData metaData, RequestDelegate handler)
    {
        switch (metaData.Method)
        {
            case ApiMethod.Get:
                _app.MapGet(metaData.Path, async context => await handler(context));
                break;
            case ApiMethod.Post:
                _app.MapPost(metaData.Path, async context => await handler(context));
                break;
            case ApiMethod.Put:
                _app.MapPut(metaData.Path, async context => await handler(context));
                break;
            case ApiMethod.Delete:
                _app.MapDelete(metaData.Path, async context => await handler(context));
                break;
            case ApiMethod.Patch:
                _app.MapPatch(metaData.Path, async context => await handler(context));
                break;
            case ApiMethod.Head:
            case ApiMethod.Options:
            case ApiMethod.Trace:
                _app.Map(metaData.Path, async context => await handler(context));
                break;
        }
    }

    public void AddRoutes(IReadOnlyDictionary<ApiMetaData, RequestDelegate> routes)
    {
        foreach (var route in routes)
        {
            AddRoute(route.Key, route.Value);
        }
    }

    public IEnumerable<string> GetListenUrls()
    {
        foreach (var (adress, _) in GetIpAdresses())
        {
            yield return $"http://{adress}:{Port}";
        }
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

    public async Task StartAsync()
        => await _app.StartAsync();

    public async Task StopAsync()
        => await _app.StopAsync();
}
