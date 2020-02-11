//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core.Configuration;
using BookGen.Framework.Server;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BookGen.Framework.Editor
{
    public class DynamicHandlers : IAdvancedRequestHandler
    {
        private readonly string _workdir;
        private readonly Config _configuruation;

        public DynamicHandlers(string workdir, Config config)
        {
            _workdir = workdir;
            _configuruation = config;
        }

        public bool CanServe(string AbsoluteUri)
        {
            return
                AbsoluteUri == "/dynamic/FileTree.html"
                || AbsoluteUri == "/dynamic/GetContents.html"
                || AbsoluteUri == "/dynamic/Save.html"
                || AbsoluteUri == "/dynamic/Toc.html";
        }

        public void Serve(HttpListenerRequest request, HttpListenerResponse response, ILog log)
        {
            switch (request.Url.AbsolutePath)
            {
                case "/dynamic/FileTree.html":
                    response.WriteHtmlString(FileTreeRenderer.Render(_workdir));
                    break;
                case "/dynamic/GetContents.html":
                    GetContents(request, response, log);
                    break;
                case "/dynamic/Save.html":
                    Save(request, response, log);
                    break;
                case "/dynamic/Toc.html":
                    Toc(response);
                    break;
            }
        }

        private void GetContents(HttpListenerRequest request, HttpListenerResponse response, ILog log)
        {
            Dictionary<string, string> parameters = request.Url.Query.ParseQueryParameters();
            if (parameters.ContainsKey("file"))
            {
                string file = Uri.UnescapeDataString(parameters["file"]);
                response.WriteString(EditorLoadSaveHelper.LoadFile(_workdir, file, log), "text/plain");
            }
        }

        private void Save(HttpListenerRequest request, HttpListenerResponse response, ILog log)
        {
            Dictionary<string, string> parameters = request.ParsePostParameters();
            if (parameters.ContainsKey("file") && parameters.ContainsKey("content"))
            {
                string file = Uri.UnescapeDataString(parameters["file"]);
                string content = Uri.UnescapeDataString(parameters["content"]);
                bool result = EditorLoadSaveHelper.SaveFile(_workdir, file, content, log);
                if (result)
                {
                    response.WriteString("OK", "text/plain");
                }
                else
                {
                    response.WriteString("Error", "text/plain");
                }
            }
        }

        private void Toc(HttpListenerResponse response)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(_configuruation.TOCFile);
            var encoded = Uri.EscapeDataString(Convert.ToBase64String(plainTextBytes));
            response.WriteString(encoded, "text/plain");
        }
    }
}
