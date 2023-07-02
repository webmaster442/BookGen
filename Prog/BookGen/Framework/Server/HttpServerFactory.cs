//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Webmaster442.HttpServerFramework;
using Webmaster442.HttpServerFramework.Handlers;

namespace BookGen.Framework.Server;

internal static class HttpServerFactory
{
    public static HttpServer CreateServerForTest(IServerLog log, string folder)
    {
        bool debug = false;
#if DEBUG
        debug = true;
#endif

        var server = new HttpServer(new HttpServerConfiguration
        {
            DebugMode = debug,
            Port = 8090,
            EnableLastAccesTime = true,
        }, log);

        server.RegisterHandler(new FileServeHandler(folder, false, server.Configuration, "/"));
        server.RegisterHandler(new QrCodeLinkHandler(server.Configuration));

        return server;
    }

    public static HttpServer CreateServerForServModule(IServerLog log, string folder)
    {
        bool debug = false;
#if DEBUG
        debug = true;
#endif

        var server = new HttpServer(new HttpServerConfiguration
        {
            DebugMode = debug,
            Port = 8081,
            EnableLastAccesTime = true,
        }, log);

        server.RegisterHandler(new FileServeHandler(folder, true, server.Configuration, "/"));
        server.RegisterHandler(new QrCodeLinkHandler(server.Configuration));

        return server;
    }

    public static HttpServer CreateServerForPreview(ILog log, IServerLog serverlog, string directory)
    {
        bool debug = false;
#if DEBUG
        debug = true;
#endif

        var server = new HttpServer(new HttpServerConfiguration
        {
            DebugMode = debug,
            Port = 8082,
            EnableLastAccesTime = false,
        }, serverlog);


        server.RegisterHandler(new PreviewStaticHandler());
        server.RegisterHandler(new PreviewRenderHandler(directory, log));
        server.RegisterHandler(new QrCodeLinkHandler(server.Configuration));

        return server;
    }
}
