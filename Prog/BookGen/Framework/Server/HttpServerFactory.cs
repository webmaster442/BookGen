//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Webmaster442.HttpServerFramework;
using Webmaster442.HttpServerFramework.Handlers;

namespace BookGen.Framework.Server
{
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
            }, log);

            server.RegisterHandler(new FileServeHandler(folder, false, "/"));

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
            }, log);

            server.RegisterHandler(new FileServeHandler(folder, true, "/"));

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
            }, serverlog);


            server.RegisterHandler(new PreviewStaticHandler());
            server.RegisterHandler(new PreviewRenderHandler(directory, log));

            return server;
        }
    }
}
