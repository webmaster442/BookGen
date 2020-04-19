//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Framework.Server;
using BookGen.Resources;
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
                { "/bootstrap.min.css", () => ResourceHandler.GetFile(KnownFile.BootstrapMinCss) },
                { "/bootstrap.min.js", () => ResourceHandler.GetFile(KnownFile.BootstrapMinJs) },
                { "/jquery.min.js", () => ResourceHandler.GetFile(KnownFile.JqueryMinJs) },
                { "/popper.min.js", () => ResourceHandler.GetFile(KnownFile.PopperMinJs)},
                { "/jsonview.css", () => ResourceHandler.GetFile(KnownFile.JsonviewCss) },
                { "/jsonview.js", () => ResourceHandler.GetFile(KnownFile.JsonviewJs) },
                { "/ace.min.js", () => ResourceHandler.GetFile(KnownFile.AceMinJs) },
                { "/keybinding-vscode.min.js", () => ResourceHandler.GetFile(KnownFile.AceKeybindingVsCodeMinJs) },
                { "/markdown.min.js", () => ResourceHandler.GetFile(KnownFile.AceMarkdownMinJs) },
                { "/mode-markdown.min.js", () => ResourceHandler.GetFile(KnownFile.AceModeMarkdownMinJs) },
                { "/theme-github.min.js", () => ResourceHandler.GetFile(KnownFile.AceThemeGithubMinJs) },
                { "/EditorApp.js", () => ResourceHandler.GetFile(KnownFile.EditorAppJs)  }

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
