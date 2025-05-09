using System.Text.RegularExpressions;

using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.JsInterop;
using Bookgen.Lib.Markdown.Renderers;

using Markdig;
using Markdig.Extensions.Figures;
using Markdig.Extensions.Tables;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Bookgen.Lib.Markdown;

public sealed class RenderSettings
{
    public required string? HostUrl { get; init; }
    public required PrismJsInterop? PrismJsInterop { get; init; }
    public required CssClasses CssClasses { get; init; }
    public required bool DeleteFirstH1 { get; init; }
    public int OffsetHeadingsBy { get; init; } = 0;
}

internal sealed partial class BookGenExtension : IMarkdownExtension, IDisposable
{
    private IImgService? _imgService;
    private RenderSettings? _settings;
    private MarkdownPipelineBuilder? _pipeline;

    [GeneratedRegex("^(\\w)+://")]
    private static partial Regex ProtocollRegex();

    public void Inject(IImgService imgService, RenderSettings settings)
    {
        _imgService = imgService;
        _settings = settings;
    }


    public void Dispose()
    {
        if (_pipeline != null)
        {
            _pipeline.DocumentProcessed -= OnDocumentProcessed;
            _pipeline = null;
        }
    }

    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        _pipeline = pipeline;
        _pipeline.DocumentProcessed += OnDocumentProcessed;
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        if (_settings == null)
            throw new InvalidOperationException();

        if (renderer is TextRendererBase<HtmlRenderer> htmlRenderer)
        {
            var linkRenderer = htmlRenderer.ObjectRenderers.FindExact<LinkInlineRenderer>();
            if (linkRenderer != null)
            {
                htmlRenderer.ObjectRenderers.Remove(linkRenderer);
                htmlRenderer.ObjectRenderers.Add(new LinkInlineRendererWithSvgSupport());
            }
            CodeBlockRenderer? codeBlockRenderer = htmlRenderer.ObjectRenderers.FindExact<CodeBlockRenderer>();
            if (codeBlockRenderer != null)
            {
                htmlRenderer.ObjectRenderers.Remove(codeBlockRenderer);
                htmlRenderer.ObjectRenderers.AddIfNotAlready(new SyntaxRenderer(codeBlockRenderer, _settings.PrismJsInterop));
            }
        }

    }

    private void OnDocumentProcessed(MarkdownDocument document)
    {
        if (_settings == null)
            throw new InvalidOperationException();

        if (_imgService == null)
            throw new InvalidOperationException();

        static void AddStyleClass(MarkdownObject node, string style)
        {
            if (string.IsNullOrEmpty(style)) return;
            node.GetAttributes().AddClass(style);
        }

        static bool IsOffHostLink(LinkInline link, string host)
        {
            if (!ProtocollRegex().IsMatch(link.Url ?? string.Empty))
                return false;

            return !link.Url?.StartsWith(host) ?? false;
        }

        bool fistH1IsDeleted = false;

        foreach (MarkdownObject node in document.Descendants())
        {
            if (node is HeadingBlock heading)
            {
                heading.Level += _settings.OffsetHeadingsBy;

                switch (heading.Level)
                {
                    case 1:
                        if (_settings.DeleteFirstH1 && !fistH1IsDeleted)
                        {
                            document.Remove(heading);
                            fistH1IsDeleted = true;
                        }
                        AddStyleClass(node, _settings.CssClasses.H1);
                        break;
                    case 2:
                        AddStyleClass(node, _settings.CssClasses.H1);
                        break;
                    case 3:
                        AddStyleClass(node, _settings.CssClasses.H3);
                        break;
                    default:
                        break;
                }
            }
            else if (node is Table)
            {
                AddStyleClass(node, _settings.CssClasses.Table);
            }
            else if (node is QuoteBlock)
            {
                AddStyleClass(node, _settings.CssClasses.Blockquote);
            }
            else if (node is Figure)
            {
                AddStyleClass(node, _settings.CssClasses.Figure);
            }
            else if (node is FigureCaption)
            {
                AddStyleClass(node, _settings.CssClasses.FigureCaption);
            }
            else if (node is LinkInline link)
            {
                if (link.IsImage)
                {
                    AddStyleClass(link, _settings.CssClasses.Img);
                    if (!string.IsNullOrEmpty(link.Url))
                    {
                        var image = _imgService.GetImageEmbedData(link.Url);
                        if (image.imageType == ImageType.Svg)
                        {
                            link.Url = image.data;
                        }
                        else
                        {
                            link.Url = $"data:{image.imageType.GetMimeType()};base64, {image.data}";
                        }
                    }
                }
                else
                {
                    AddStyleClass(link, _settings.CssClasses.Link);
                    if (!string.IsNullOrEmpty(_settings.HostUrl)
                        && IsOffHostLink(link, _settings.HostUrl))
                    {
                        link.GetAttributes().AddProperty("target", "_blank");
                    }
                }
            }
            else if (node is ListBlock listBlock)
            {
                if (listBlock.IsOrdered)
                    AddStyleClass(listBlock, _settings.CssClasses.Ol);
                else
                    AddStyleClass(listBlock, _settings.CssClasses.Ul);
            }
            else if (node is ListItemBlock)
            {
                AddStyleClass(node, _settings.CssClasses.Li);
            }
        }
    }
}
