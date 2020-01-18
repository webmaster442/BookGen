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
                || AbsoluteUri == "/editor.html";
        }

        public void Serve(string AbsoluteUri, HttpListenerResponse response, ILog log)
        {
            if (AbsoluteUri == "/" || AbsoluteUri == "/index.html")
            {
                var str = ResourceLocator.GetResourceFile<BuiltInTemplates>("/Editor/Index.html");
                response.WriteHtmlString(str);
            }
            else if (AbsoluteUri == "/editor.html")
            {
                var str = ResourceLocator.GetResourceFile<BuiltInTemplates>("/Editor/Editor.html");
                response.WriteHtmlString(str);
            }
        }
    }
}
