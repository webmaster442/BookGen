//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Framework.Server;
using BookGen.Resources;
using System.Net;

namespace BookGen.Framework.Editor
{
    internal class HtmlPageHandler : ISimpleRequestHandler
    {
        public bool CanServe(string AbsoluteUri)
        {
            return
                AbsoluteUri == "/"
                || AbsoluteUri == "/index.html"
                || AbsoluteUri == "/editor.html"
                || AbsoluteUri == "/config.html";
        }

        public void Serve(string AbsoluteUri, HttpListenerResponse response, ILog log)
        {
            string str = "";

            if (AbsoluteUri == "/" || AbsoluteUri == "/index.html")
            {
                str = ResourceHandler.GetFile(KnownFile.IndexHtml);
            }
            else if (AbsoluteUri == "/editor.html")
            {
                str = ResourceHandler.GetFile(KnownFile.EditorHtml);
            }
            else if (AbsoluteUri == "/config.html")
            {
                str = ResourceHandler.GetFile(KnownFile.ConfigViewHtml);
            }
            response.WriteHtmlString(str);
        }
    }
}
