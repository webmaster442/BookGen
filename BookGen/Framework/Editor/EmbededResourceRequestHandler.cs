//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Framework.Server;
using BookGen.Template;
using System;
using System.Collections.Generic;
using System.Net;

namespace BookGen.Framework.Editor
{
    public class EmbededResourceRequestHandler : ISimpleRequestHandler
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
                { "/jsonview.css", () => ResourceLocator.GetResourceFile<BuiltInTemplates>("/Editor/jsonview.css") },
                { "/jsonview.js", () => ResourceLocator.GetResourceFile<BuiltInTemplates>("/Editor/jsonview.js") },
            };
        }

        public bool CanServe(string AbsoluteUri)
        {
            return _knownFiles.ContainsKey(AbsoluteUri);
        }

        public void Serve(string AbsoluteUri, HttpListenerResponse response, ILog log)
        {
            var str = _knownFiles[AbsoluteUri].Invoke();
            response.WriteString(str, MimeTypes.GetMimeTypeForFile(AbsoluteUri));
        }
    }
}
