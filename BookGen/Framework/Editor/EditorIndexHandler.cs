//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Framework.Server;
using BookGen.Template;
using System.Net;
using System.Text;

namespace BookGen.Framework.Editor
{
    internal class EditorIndexHandler : IRequestHandler
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
            byte[] content = Encoding.UTF8.GetBytes(str);

            response.ContentEncoding = Encoding.UTF8;
            response.ContentType = MimeTypes.GetMimeForExtension(".html");
            response.OutputStream.Write(content, 0, content.Length);
        }
    }
}
