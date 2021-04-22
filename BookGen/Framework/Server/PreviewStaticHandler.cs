//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Resources;
using System.Collections.Generic;
using System.Net;

namespace BookGen.Framework.Server
{
    internal class PreviewStaticHandler : ISimpleRequestHandler
    {
        private readonly Dictionary<string, KnownFile> _table;

        public PreviewStaticHandler()
        {
            _table = new()
            {
                { "/prism.js", KnownFile.PrismJs },
                { "/prism.css", KnownFile.PrismCss },
                { "/preview.css", KnownFile.PreviewCss },
            };
        }


        public bool CanServe(string AbsoluteUri)
        {
            return _table.ContainsKey(AbsoluteUri);
        }

        public void Serve(string AbsoluteUri, HttpListenerResponse response, ILog log)
        {
            var file = ResourceHandler.GetFile(_table[AbsoluteUri]);
            response.WriteString(file, MimeTypes.GetMimeTypeForFile(AbsoluteUri));
        }
    }
}
