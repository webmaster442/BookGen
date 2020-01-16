using BookGen.Framework.Server;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BookGen.Framework.Editor
{
    public class DynamicHandlers : IAdvancedRequestHandler
    {
        public bool CanServe(string AbsoluteUri)
        {
            return AbsoluteUri == "/dynamic/FileTree.html";
        }

        public void Serve(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (request.Url.AbsolutePath == "/dynamic/FileTree.html")
            {

            }
        }
    }
}
