//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Core.Markdown;

namespace BookGen.Framework.Server
{
    internal sealed class PreviewRenderHandler : ISimpleRequestHandler, IDisposable
    {

        private readonly string _directory;
        private readonly ILog _log;
        private FileSystemWatcher? _fsw;
        private HashSet<string> _dirFiles;
        private readonly TemplateProcessor _processor;
        private readonly BookGenPipeline _mdpipeline;


        public PreviewRenderHandler(string directory, ILog log)
        {
            _directory = directory;
            _log = log;

            _fsw = new FileSystemWatcher(_directory, "*.md");
            _fsw.Created += OnRefreshDir;
            _fsw.Deleted += OnRefreshDir;
            _fsw.Renamed += OnRefreshDir;
            _fsw.EnableRaisingEvents = true;
            _dirFiles = new HashSet<string>(Directory.GetFiles(_directory, "*.md"));

            _processor = new TemplateProcessor(new Core.Configuration.Config(),
                             new ShortCodeParser(new List<ITemplateShortCode>(),
                                                 new Scripts.CsharpScriptHandler(_log),
                                                 new Core.Configuration.Translations(),
                                                 _log),
                             new StaticTemplateContent());

            _processor.TemplateContent = ResourceHandler.GetFile(KnownFile.PreviewHtml);
            _mdpipeline = new BookGenPipeline(BookGenPipeline.Preview);
            _mdpipeline.SetSyntaxHighlightTo(false);
        }



        public bool CanServe(string AbsoluteUri)
        {
            return CanServeFromDir(AbsoluteUri, out _)
                || AbsoluteUri == "/";
        }

        private bool CanServeFromDir(string absoluteUri, out string foundUri)
        {
            string proble = absoluteUri;
            if (absoluteUri.StartsWith("/"))
            {
                proble = absoluteUri[1..];
            }

            foundUri = _dirFiles.FirstOrDefault(x => x.EndsWith(proble)) ?? string.Empty;
            return !string.IsNullOrEmpty(foundUri);
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
            _log.Info("Refreshing file list of: {0}...", _directory);
            _dirFiles = new HashSet<string>(Directory.GetFiles(_directory, "*.md"));
        }

        public void Serve(string AbsoluteUri, HttpListenerResponse response, ILog log)
        {
            if (AbsoluteUri == "/")
            {
                _log.Info("Serving index...");
                _processor.Content = WriteIndex();
                _processor.Title = "Preview";
                response.WriteHtmlString(_processor.Render());
            }
            else if (CanServeFromDir(AbsoluteUri, out string found))
            {
                _processor.Title = $"Preview of {AbsoluteUri}";
                FsPath path = new FsPath(found);
                _processor.Content = _mdpipeline.RenderMarkdown(path.ReadFile(log));
                response.WriteHtmlString(_processor.Render());
            }
        }

        public string WriteIndex()
        {
            StringBuilder html = new StringBuilder();
            html.WriteHeader(1, "Index of: {0}", _directory);
            html.WriteElement(HtmlElement.Table);
            html.WriteTableHeader("File name", "Actions");
            foreach (var file in _dirFiles)
            {
                html.WriteTableRow(Path.GetFileName(file), GetLink(file));
            }
            html.CloseElement(HtmlElement.Table);
            return html.ToString();
        }

        private static string GetLink(string file)
        {
            return $"<a target=\"_blank\" href=\"{Path.GetFileName(file)}\">Preview</a>";
        }
    }
}
