//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Framework.Server;
using System;
using System.Net;

namespace BookGen.Tests.Environment
{
    internal class UpdateTestStreamHandler : ISimpleRequestHandler
    {
        public bool CanServe(string AbsoluteUri)
        {
            return AbsoluteUri == "/download";
        }

        public void Serve(string AbsoluteUri, HttpListenerResponse response, ILog log)
        {
            response.StatusCode = 200;
            response.ContentType = "text/plain";
            const int FiveMegaBytes = 5 * 1024;

            byte[] buffer = new byte[1024];
            Array.Fill<byte>(buffer, (byte)'a');

            for (int i=0; i<FiveMegaBytes; i++)
            {
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
