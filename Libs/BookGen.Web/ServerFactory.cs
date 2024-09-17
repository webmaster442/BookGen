//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Web;
public static class ServerFactory
{
    public static IHttpServer CreateServerForHosting(string directoryToServe)
    {
        var server = new HttpServer(8080);
        server.AddStaticFiles(directoryToServe, "/");
        return server;
    }
}
