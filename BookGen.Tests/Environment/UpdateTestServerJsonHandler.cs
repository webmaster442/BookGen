//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework.Server;
using System.IO;
using System.Net;

namespace BookGen.Tests.Environment
{
    internal class UpdateTestServerJsonHandler : IRequestHandler
    {
        public bool CanServe(string AbsoluteUri)
        {
            return AbsoluteUri == "/updatejson";
        }

        public void Serve(string AbsoluteUri, HttpListenerResponse response)
        {
            response.ContentType = "application/json";
            response.StatusCode = 200;

            using (var file = File.OpenRead(TestEnvironment.GetFile("update.json")))
            {
                file.CopyTo(response.OutputStream);
            }
        }
    }
}
