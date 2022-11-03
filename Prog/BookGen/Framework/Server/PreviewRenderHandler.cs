//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;
using BookGen.DomainServices.Markdown;
using BookGen.Interfaces;
using BookGen.Resources;
using System.IO;
using Webmaster442.HttpServerFramework;
using Webmaster442.HttpServerFramework.Domain;

namespace BookGen.Framework.Server
{
    internal sealed class PreviewRenderHandler : IRequestHandler, IDisposable
    {

        private readonly string _directory;
        private readonly ILog _log;
        private readonly TemplateProcessor _processor;
        private readonly PreviewIndexBuilder _indexBuilder;
        private BookGenPipeline? _mdpipeline;


        public PreviewRenderHandler(string directory, ILog log)
        {
            _directory = directory;
            _log = log;
            _indexBuilder = new(directory);

            _processor = new TemplateProcessor(new Config(),
                             new ShortCodeParser(new List<ITemplateShortCode>(),
                                                 new Scripts.CsharpScriptHandler(_log),
                                                 new Translations(),
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
            if (absoluteUri.StartsWith('/'))
                absoluteUri = absoluteUri.Substring(1);

            //string filePath = absoluteUri.Replace("/", "\\");
            string checkPath = Path.Combine(_directory, absoluteUri);

            foundUri = checkPath;
            return File.Exists(checkPath);
        }

        public void Dispose()
        {
            if (_mdpipeline != null)
            {
                _mdpipeline.Dispose();
                _mdpipeline = null;
            }
        }

        public async Task<bool> Handle(IServerLog? log, HttpRequest request, HttpResponse response)
        {
            response.AdditionalHeaders.Add("Cache-Control", "no-store");
            response.ContentType = "text/html";
            if (request.Url == "/")
            {
                _log.Info("Serving index...");
                _processor.Content = _indexBuilder.RenderIndex();
                _processor.Title = "Preview";
                await response.Write(_processor.Render());
                return true;
            }
            else if (CanServeFromDir(request.Url, out string found)
                     && log is ILog bookGenLog)
            {
                _processor.Title = $"Preview of {request.Url}";
                var path = new FsPath(found);
                _processor.Content = _mdpipeline?.RenderMarkdown(path.ReadFile(bookGenLog)) ?? string.Empty;
                await response.Write(_processor.Render());
                return true;
            }
            return false;
        }
    }
}
