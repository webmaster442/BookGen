//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Resources;

using Webmaster442.HttpServerFramework;
using Webmaster442.HttpServerFramework.Handlers;

namespace BookGen.Framework.Server;

internal static class HttpServerFactory
{
    public static HttpServer CreateServerForTest(ILog log, string folder)
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

        server
            .RegisterHandler(new FaviconHandler(ResourceHandler.GetFileStream(KnownFile.FaviconT),
                                                MimeTypes.GetMimeForExtension(".png")))
            .RegisterHandler(new FileServeHandler(folder, false, server.Configuration, "/"))
            .RegisterHandler(new QrCodeLinkHandler(server.Configuration));

        return server;
    }

    public static HttpServer CreateServerForServModule(ILog log, string folder)
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

        server
            .RegisterHandler(new FileServeHandler(folder, true, server.Configuration, "/"))
            .RegisterHandler(new FaviconHandler(ResourceHandler.GetFileStream(KnownFile.FaviconFs),
                                                MimeTypes.GetMimeForExtension(".png")))
            .RegisterHandler(new QrCodeLinkHandler(server.Configuration));

        return server;
    }

    public static HttpServer CreateServerForPreview(ILog log, string directory)
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
        }, log);


        server
            .RegisterHandler(new FaviconHandler(ResourceHandler.GetFileStream(KnownFile.FaviconP),
                                                MimeTypes.GetMimeForExtension(".png")))
            .RegisterHandler(new PreviewStaticHandler())
            .RegisterHandler(new PreviewRenderHandler(directory, log))
            .RegisterHandler(new QrCodeLinkHandler(server.Configuration));

        return server;
    }
}
