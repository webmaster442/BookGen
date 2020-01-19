//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Framework.Server;
using BookGen.Template;
using System.Net;

namespace BookGen.Framework.Editor
{
    internal class EditorIndexHandler : ISimpleRequestHandler
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
                str = ResourceLocator.GetResourceFile<BuiltInTemplates>("/Editor/Index.html");
            }
            else if (AbsoluteUri == "/editor.html")
            {
                str = ResourceLocator.GetResourceFile<BuiltInTemplates>("/Editor/Editor.html");
            }
            else if (AbsoluteUri == "/config.html")
            {
                str = ResourceLocator.GetResourceFile<BuiltInTemplates>("/Editor/ConfigView.html");
            }
            response.WriteHtmlString(str);
        }
    }
}
