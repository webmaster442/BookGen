//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Core.Markdown;
using BookGen.Framework.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BookGen.Framework.Editor
{
    public class DynamicHandlers : IAdvancedRequestHandler
    {
        private readonly string _workdir;
        private readonly Config _configuruation;
        private readonly FileExplorerHelper _fileExplorerHelper;

        public DynamicHandlers(string workdir, Config config)
        {
            _workdir = workdir;
            _configuruation = config;
            _fileExplorerHelper = new FileExplorerHelper();
            _fileExplorerHelper.ExcludedPaths.AddRange(new string[]
            {
                new FsPath(workdir, _configuruation.TargetEpub.OutPutDirectory).ToString(),
                new FsPath(workdir, _configuruation.TargetPrint.OutPutDirectory).ToString(),
                new FsPath(workdir, _configuruation.TargetWeb.OutPutDirectory).ToString(),
                new FsPath(workdir, _configuruation.TargetWordpress.OutPutDirectory).ToString()
            });
        }

        public bool CanServe(string AbsoluteUri)
        {
            return
                AbsoluteUri == "/dynamic/FileTree.html"
                || AbsoluteUri == "/dynamic/GetContents.html"
                || AbsoluteUri == "/dynamic/Save.html"
                || AbsoluteUri == "/dynamic/Toc.html"
                || AbsoluteUri == "/dynamic/Preview.html";
        }

        public void Serve(HttpListenerRequest request, HttpListenerResponse response, ILog log)
        {
            switch (request.Url.AbsolutePath)
            {
                case "/dynamic/FileTree.html":
                    response.WriteHtmlString(_fileExplorerHelper.RenderFileExplorer(_workdir));
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
                case "/dynamic/Preview.html":
                    Preview(request, response);
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

        private void Preview(HttpListenerRequest request, HttpListenerResponse response)
        {
            Dictionary<string, string> parameters = request.ParsePostParameters();
            if (parameters.ContainsKey("content"))
            {
                string base64content = Uri.UnescapeDataString(parameters["content"]);

                byte[] contentBytes = Convert.FromBase64String(base64content);
                string rawContent = Encoding.UTF8.GetString(contentBytes);

                string rendered = MarkdownRenderers.Markdown2Preview(rawContent, new FsPath(_workdir));
                response.WriteString(rendered, "text/html");
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
