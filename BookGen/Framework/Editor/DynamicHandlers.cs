//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework.Server;
using System.Collections.Generic;
using System.Net;

namespace BookGen.Framework.Editor
{
    public class DynamicHandlers : IAdvancedRequestHandler
    {
        private readonly string _workdir;

        public DynamicHandlers(string workdir)
        {
            _workdir = workdir;
        }

        public bool CanServe(string AbsoluteUri)
        {
            return
                AbsoluteUri == "/dynamic/FileTree.html"
                || AbsoluteUri == "/dynamic/GetContents.html";
        }

        public void Serve(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (request.Url.AbsolutePath == "/dynamic/FileTree.html")
            {
                response.WriteHtmlString(FileTreeRenderer.Render(_workdir));
            }
            else if (request.Url.AbsolutePath == "/dynamic/GetContents.html")
            {
                Dictionary<string, string> parameters = request.Url.Query.ParseQueryParameters();
                string file = parameters["file"];
                response.WriteString(EditorLoadSave.LoadFile(_workdir, file), "text/plain");

            }
        }
    }
}
