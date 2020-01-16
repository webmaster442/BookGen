//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
                || AbsoluteUri == "/index.html";
        }

        public void Serve(string AbsoluteUri, HttpListenerResponse response)
        {
            var str = ResourceLocator.GetResourceFile<BuiltInTemplates>("/Editor/Index.html");
            response.WriteHtmlString(str);
        }
    }
}
