//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Framework.Server;
using BookGen.Template;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BookGen.Framework.Editor
{
    public class EmbededResourceRequestHandler : IRequestHandler
    {
        private readonly Dictionary<string, Func<string>> _knownFiles;

        public EmbededResourceRequestHandler()
        {
            _knownFiles = new Dictionary<string, Func<string>>
            {
                { "/bootstrap.min.css", () => BuiltInTemplates.AssetBootstrapCSS },
                { "/bootstrap.min.js", () => BuiltInTemplates.AssetBootstrapJs },
                { "/jquery.min.js", () => BuiltInTemplates.AssetJqueryJs },
                { "/popper.min.js", () => BuiltInTemplates.AssetPopperJs },
                { "/simplemde.min.css", () => ResourceLocator.GetResourceFile<BuiltInTemplates>("/Editor/simplemde.min.css") },
                { "/simplemde.min.js", () => ResourceLocator.GetResourceFile<BuiltInTemplates>("/Editor/simplemde.min.js") },
            };
        }

        public bool CanServe(string AbsoluteUri)
        {
            return _knownFiles.ContainsKey(AbsoluteUri);
        }

        public void Serve(string AbsoluteUri, HttpListenerResponse response)
        {
            response.StatusCode = 200;

            var str = _knownFiles[AbsoluteUri].Invoke();
            byte[] content = Encoding.UTF8.GetBytes(str);

            response.ContentEncoding = Encoding.UTF8;
            response.ContentType = MimeTypes.GetMimeTypeForFile(AbsoluteUri);
            response.OutputStream.Write(content, 0, content.Length);
        }
    }
}
