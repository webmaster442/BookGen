//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BookGen.Framework.Server
{
    internal sealed class PreviewFilesHandler : ISimpleRequestHandler, IDisposable
    {
        private readonly Dictionary<string, KnownFile> _table;
        private readonly string _directory;
        private readonly ILog _log;
        private FileSystemWatcher? _fsw;
        private HashSet<string> _dirFiles;

        public PreviewFilesHandler(string directory, ILog log)
        {
            _directory = directory;
            _log = log;
            _table = new()
            {
                { "prism.js", KnownFile.PrismJs },
                { "prism.css", KnownFile.PrismCss },
                { "preview.css", KnownFile.PreviewCss },
            };

            _fsw = new FileSystemWatcher(_directory, "*.md");
            _fsw.Created += OnRefreshDir;
            _fsw.Deleted += OnRefreshDir;
            _fsw.Renamed += OnRefreshDir;
            _fsw.EnableRaisingEvents = true;
            _dirFiles = new HashSet<string>(Directory.GetFiles(_directory, "*.md"));
        }



        public bool CanServe(string AbsoluteUri)
        {
            return _table.ContainsKey(AbsoluteUri) 
                || _dirFiles.Any(x => x.EndsWith(AbsoluteUri))
                || AbsoluteUri == "/";
        }

        public void Dispose()
        {
            if (_fsw != null)
            {
                _fsw.EnableRaisingEvents = false;
                _fsw.Dispose();
                _fsw = null;
            }
        }

        private void OnRefreshDir(object sender, FileSystemEventArgs e)
        {
            _dirFiles = new HashSet<string>(Directory.GetFiles(_directory, "*.md"));
        }

        public void Serve(string AbsoluteUri, HttpListenerResponse response, ILog log)
        {
            if (_table.ContainsKey(AbsoluteUri))
            {
                var file = ResourceHandler.GetFile(_table[AbsoluteUri]);
                response.WriteString(file, MimeTypes.GetMimeTypeForFile(AbsoluteUri));
            }
            else if (AbsoluteUri == "/")
            {
            }
        }

        public string WriteIndex()
        {
            StringBuilder html = new StringBuilder();
            html.WriteElement(HtmlElement.Table);
            html.WriteTableHeader("File name", "Actions");
            foreach (var file in _dirFiles)
            {
                html.WriteTableRow(Path.GetFileName(file), GetLink(file));
            }
            html.CloseElement(HtmlElement.Table);
            return html.ToString();
        }

        private string GetLink(string file)
        {
            return $"<a href=\"{Path.GetFileName(file)}\">Preview</a>";
        }
    }
}
