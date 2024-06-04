//-----------------------------------------------------------------------------
// (c) 2021-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Web;

using BookGen.Domain.Configuration;
using BookGen.DomainServices.Markdown;
using BookGen.RenderEngine;
using BookGen.Resources;

using Webmaster442.HttpServerFramework;
using Webmaster442.HttpServerFramework.Domain;

namespace BookGen.Framework.Server;

internal sealed class PreviewRenderHandler : IRequestHandler, IDisposable
{
    private readonly string _directory;
    private readonly ILog _log;
    private readonly TemplateProcessor _processor;
    private readonly PreviewIndexBuilder _indexBuilder;
    private BookGenPipeline? _mdpipeline;

    public PreviewRenderHandler(string directory, ILog log, IAppSetting appSetting)
    {
        _directory = directory;
        _log = log;
        _indexBuilder = new(directory);

        var functionServices = new FunctionServices
        {
            Log = log,
            TimeProvider = TimeProvider.System,
            RuntimeSettings = new RuntimeSettings(new EmptyTagUtils()),
            AppSetting = appSetting,
        };

        _processor = new TemplateProcessor(functionServices, new StaticTemplateContent());
        _processor.TemplateContent = ResourceHandler.GetFile(KnownFile.PreviewHtml);
        _mdpipeline = new BookGenPipeline(BookGenPipeline.Preview);
        _mdpipeline.SetSyntaxHighlightTo(false);
    }

    public bool CanServe(string absoluteUri)
    {
        return CanServeFromDir(absoluteUri, out _)
            || absoluteUri == "/";
    }

    private bool CanServeFromDir(string absoluteUri, out string foundUri)
    {
        if (absoluteUri.StartsWith('/'))
            absoluteUri = absoluteUri[1..];

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

    public async Task<bool> Handle(ILog? log, HttpRequest request, HttpResponse response)
    {
        response.Headers.Add("Cache-Control", "no-store");
        response.ContentType = "text/html";
        if (request.Url == "/")
        {
            _log.Info("Serving index...");
            _processor.TemplateContent = ResourceHandler.GetFile(KnownFile.PreviewHtml);
            _processor.Content = _indexBuilder.RenderIndex();
            _processor.Title = "Preview";
            await response.WriteAsync(_processor.Render());
            return true;
        }
        else if (CanServeFromDir(request.Url, out string found))
        {
            var fileContents = new FsPath(found).ReadFile(_log);

            _mdpipeline?.InjectPath(new FsPath(_directory));

            if (request.Parameters.ContainsKey("edit")
                && request.Parameters["edit"] == "true")
            {
                _processor.AddContent("filename", found);
                _processor.TemplateContent = ResourceHandler.GetFile(KnownFile.EditHtml);
                _processor.Title = $"Editing {request.Url}";
                _processor.Content = fileContents;

                if (request.Method == RequestMethod.Post)
                {
                    HandleSave(request.RequestContent, new FsPath(found), fileContents);
                }
            }
            else
            {
                _processor.TemplateContent = ResourceHandler.GetFile(KnownFile.PreviewHtml);
                _processor.Title = $"Preview of {request.Url}";
                _processor.Content = _mdpipeline?.RenderMarkdown(fileContents) ?? string.Empty;
            }
            await response.WriteAsync(_processor.Render());
            return true;
        }
        return false;
    }

    private void HandleSave(byte[] requestContent, FsPath fileToSave, string fileContents)
    {
        const string fieldCheck = "editor=";
        string content = HttpUtility.UrlDecode(requestContent, Encoding.UTF8);
        if (!content.StartsWith(fieldCheck))
        {
            _log.Warning("Failed to save file: {0}", fileToSave);
            return;
        }

        string data = content[fieldCheck.Length..];
        if (data != fileContents)
        {
            fileToSave.WriteFile(_log, data);
            return;
        }

        _log.Info("Requested file save, but content hasn't been changed for: {0}", fileToSave);
    }
}
