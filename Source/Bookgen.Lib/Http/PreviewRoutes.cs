using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mime;

using BookGen.Lib.AppSettings;
using BookGen.Lib.Domain.IO.Configuration;
using BookGen.Lib.Rendering;
using BookGen.Lib.Rendering.Images;
using BookGen.Lib.Rendering.Markdown;
using BookGen.Lib.Rendering.Markdown.RenderInterop;
using BookGen.Lib.Rendering.Templates;
using BookGen.Vfs;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BookGen.Lib.Http;

internal sealed class PreviewRoutes : IDisposable, IRouteProvider
{
    private readonly IReadOnlyFileSystem _source;
    private readonly ILogger _logger;
    private readonly MarkdownRenderSettings _renderSettings;
    private readonly MarkdownConverter _markdownConverter;
    private readonly TemplateEngine _templateEngine;
    private readonly string _template;

    private readonly IFileSystemObserver _observer;
    private readonly List<string> _allowedFiles;
    private readonly Lock _lock;

    public PreviewRoutes(IReadOnlyFileSystem source,
                         ILogger logger,
                         IProgramPathResolver programPathResolver,
                         IAssetSource assetSource)
    {
        _allowedFiles = new List<string>();
        _lock = new Lock();
        _source = source;
        _logger = logger;
        var imageConfig = new ImageConfig
        {
            SvgRecode = SvgRecodeOption.Passtrough,
            ResizeAndRecodeImages = ImgRecodeOption.Passtrough,
        };

        var imgService = new ImgService(source, _logger, imageConfig);

        _renderSettings = new MarkdownRenderSettings(imgService)
        {
            HostUrl = "http://localhost",
            DeleteFirstH1 = false,
            CssClasses = new CssClasses(),
            OffsetHeadingsBy = 0,
            AutoEmbedSupportedLinks = true,
            RenderInterop = new RenderInterop(assetSource, programPathResolver, imageConfig)
        };

        _markdownConverter = new MarkdownConverter(_renderSettings);
        _templateEngine = new TemplateEngine(_logger, assetSource);
        _template = assetSource.GetAsset(BundledAssets.TemplateSinglePage);

        _observer = _source.CreateObserver(logger, "*.*");
        _observer.FileChanged += OnFileChange;
        _allowedFiles.Clear();
        _allowedFiles.AddRange(_source.GetFiles(_source.Scope, "*.md", true));
    }

    private void OnFileChange(object? sender, FileSystemChangeEventArgs e)
    {
        lock (_lock)
        {
            switch (e.ChangeType)
            {
                case FileSystemChangeEventArgs.Change.Created:
                    _allowedFiles.Add(e.FileName);
                    break;
                case FileSystemChangeEventArgs.Change.Changed:
                    break;
                case FileSystemChangeEventArgs.Change.Renamed:
                    int oldindex = _allowedFiles.IndexOf(e.FileName);
                    if (oldindex != -1)
                    {
                        _allowedFiles[oldindex] = e.FileName;
                    }
                    else
                    {
                        _allowedFiles.Add(e.FileName);
                    }
                    break;
                case FileSystemChangeEventArgs.Change.Deleted:
                    _allowedFiles.Remove(e.FileName);
                    break;
            }
        }
    }

    public void Dispose()
    {
        _observer.FileChanged -= OnFileChange;
        _observer.Dispose();
        _markdownConverter.Dispose();
        _renderSettings.Dispose();
    }

    public IEnumerable<KeyValuePair<ApiMetaData, RequestDelegate>> Routes
    {
        get
        {
            var metaData = new ApiMetaData("/preview", MediaTypeNames.Text.Html, ApiMethod.Get);
            yield return new KeyValuePair<ApiMetaData, RequestDelegate>(metaData, RenderPreview);
        }
    }

    private async Task RenderPreview(HttpContext context)
    {
        string? fileName = context.Request.Query["file"];
        if (!CanServe(fileName))
        {
            var viewData = new ViewData
            {
                Host = string.Empty,
                Content = "Not a markdown file",
                Title = "Error",
                LastModified = DateTime.UtcNow,
            };
            await SendData(context,
                           HttpStatusCode.BadRequest,
                           _templateEngine.Render(_template, viewData),
                           MediaTypeNames.Text.Html);

            return;
        }

        var source = await _source.GetSourceFile(fileName, _logger);

        var data = new ViewData
        {
            Content = _markdownConverter.RenderMarkdownToHtml(source.Content),
            Title = source.FrontMatter.Title,
            LastModified = source.LastModified,
            Host = "http://localhost"
        };

        await SendData(context,
                       HttpStatusCode.OK,
                       _templateEngine.Render(_template, data),
                       MediaTypeNames.Text.Html);

    }

    private bool CanServe([NotNullWhen(true)] string? fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return false;

        return _allowedFiles.Contains(fileName);
    }

    private static async Task SendData(HttpContext httpContext, HttpStatusCode httpStatusCode, string data, string mimeType)
    {
        httpContext.Response.StatusCode = (int)httpStatusCode;
        httpContext.Response.ContentType = mimeType;
        await httpContext.Response.WriteAsync(data);
    }
}
