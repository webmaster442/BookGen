//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.RenderEngine;

namespace BookGen.Framework;

internal sealed class TemplateProcessor : ITemplateProcessor
{
    private readonly TemplateRenderer _templateRenderer;
    private readonly TemplateParameters _parameters;

    public string TemplateContent { get; set; }

    public TemplateProcessor(FunctionServices functionServices,
                             StaticTemplateContent? staticContent = null)
    {
        _templateRenderer = new TemplateRenderer(functionServices);
        var hostname = functionServices.RuntimeSettings.Configuration.HostName;
        _parameters = new TemplateParameters
        {
            TocHtml = staticContent != null ? staticContent.TableOfContentsHtml : string.Empty,
            Toc = staticContent != null ? staticContent.TableOfContents : string.Empty,
            Title = staticContent != null ? staticContent.Title : string.Empty,
            Content = staticContent != null ? staticContent.Content : string.Empty,
            Host = !string.IsNullOrEmpty(hostname) ? hostname : string.Empty,
            Metadata = staticContent != null ? staticContent.Metadata : string.Empty,
            PrecompiledHeader = staticContent != null ? staticContent.PrecompiledHeader : string.Empty,
        };
        foreach (var translation in functionServices.RuntimeSettings.Configuration.Translations)
        {
            _parameters.Add(translation.Key, translation.Value);
        }

        TemplateContent = string.Empty;
    }

    public string Content
    {
        get => _parameters.Content;
        set => _parameters.Content = value;
    }

    public string Title
    {
        get => _parameters.Title;
        set => _parameters.Title = value;
    }

    public string TableOfContents
    {
        get => _parameters.Toc;
        set => _parameters.Toc = value;
    }

    public string TableOfContentsHtml
    {
        get => _parameters.TocHtml;
        set => _parameters.TocHtml = value;
    }

    public string Metadata
    {
        get => _parameters.Metadata;
        set => _parameters.Metadata = value;
    }

    public string HostUrl => _parameters.Host;

    public string PrecompiledHeader
    {
        get => _parameters.PrecompiledHeader;
        set => _parameters.PrecompiledHeader = value;
    }

    public string Render()
    {
        if (TemplateContent == null)
            throw new InvalidOperationException("Can't generate while TemplateContent is null");

        return _templateRenderer.Render(TemplateContent, _parameters);
    }

    public void AddContent(string key, string value)
    {
        _parameters.Add(key, value);
    }
}
