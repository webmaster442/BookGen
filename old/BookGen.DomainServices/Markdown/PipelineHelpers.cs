//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

using BookGen.DomainServices.Markdown.Renderers;
using BookGen.Interfaces;
using BookGen.Interfaces.Configuration;

using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace BookGen.DomainServices.Markdown
{
    internal static class PipelineHelpers
    {
        public static void AppendPrismCss(MarkdownDocument document, bool isPrinting = false)
        {
            var content = new StringBuilder();
            content.AppendLine("<style type=\"text/css\">");

            if (isPrinting)
                content.Append(Resources.ResourceHandler.GetFile(Resources.KnownFile.PrismPrintCss));
            else
                content.Append(Resources.ResourceHandler.GetFile(Resources.KnownFile.PrismCss));

            content.AppendLine("</style>");
            var block = new HtmlBlock(new HtmlBlockParser());
            block.Lines = new Markdig.Helpers.StringLineGroup(content.ToString());
            document.Insert(0, block);
        }

        public static void SetupLinkInlineRendererWithSvgSupport(IMarkdownRenderer renderer)
        {
            if (renderer is not TextRendererBase<HtmlRenderer> htmlRenderer) return;

            var original = htmlRenderer.ObjectRenderers.FindExact<LinkInlineRenderer>();
            if (original != null)
            {
                htmlRenderer.ObjectRenderers.Remove(original);
                htmlRenderer.ObjectRenderers.Add(new LinkInlineRendererWithSvgSupport());
            }
        }

        public static void SetupSyntaxRenderForPreRender(IMarkdownRenderer renderer, JavaScriptInterop interop)
        {
            if (renderer is not TextRendererBase<HtmlRenderer> htmlRenderer) return;

            CodeBlockRenderer? originalCodeBlockRenderer = htmlRenderer.ObjectRenderers.FindExact<CodeBlockRenderer>();
            if (originalCodeBlockRenderer != null)
            {
                htmlRenderer.ObjectRenderers.Remove(originalCodeBlockRenderer);
                htmlRenderer.ObjectRenderers.AddIfNotAlready(new Renderers.SyntaxRenderer(originalCodeBlockRenderer, interop));
            }
        }

        public static void SetupSyntaxRenderForWeb(IMarkdownRenderer renderer)
        {
            if (renderer is not TextRendererBase<HtmlRenderer> htmlRenderer) return;
            CodeBlockRenderer? originalCodeBlockRenderer = htmlRenderer.ObjectRenderers.FindExact<CodeBlockRenderer>();
            if (originalCodeBlockRenderer != null)
            {
                htmlRenderer.ObjectRenderers.Remove(originalCodeBlockRenderer);
                htmlRenderer.ObjectRenderers.AddIfNotAlready(new Renderers.TerminalOutputSyntaxRenderer(originalCodeBlockRenderer));
            }
        }


        internal static void DeleteFirstH1(MarkdownDocument document)
        {
            HeadingBlock? title = null;
            foreach (MarkdownObject? node in document.Descendants())
            {
                if (node is HeadingBlock heading && heading.Level == 1)
                {
                    title = heading;
                    break;
                }
            }

            if (title != null)
                document.Remove(title);
        }

        private static void AddStyleClass(MarkdownObject node, string style)
        {
            if (string.IsNullOrEmpty(style)) return;
            node.GetAttributes().AddClass(style);
        }

        public static void RenderImages(IReadonlyRuntimeSettings runtimeConfig,
                                        MarkdownDocument document)
        {
            foreach (MarkdownObject? node in document.Descendants())
            {
                if (node is LinkInline link
                    && link.IsImage
                    && !string.IsNullOrEmpty(link.Url))
                {
                    link.Url = FixExtension(link.Url, runtimeConfig.CurrentBuildConfig.ImageOptions);
                    if (runtimeConfig.InlineImgCache?.Count > 0)
                    {
                        string? inlinekey = Path.GetFileName(link.Url)
                            ?? throw new InvalidOperationException($"Couldn't get file name for: {link.Url}");

                        if (runtimeConfig.InlineImgCache.TryGetValue(inlinekey, out string? value))
                        {
                            link.Url = value;
                        }
                    }
                }
            }
        }

        private static string? FixExtension(string? url, IReadonlyImageOptions imageOptions)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            string extension = Path.GetExtension(url);
            if (extension == ".svg")
            {
                if (imageOptions.SvgPassthru)
                    return url;

                return Path.ChangeExtension(url, ".png");
            }
            else if (extension == ".png" && imageOptions.RecodePngToWebp)
            {
                return Path.ChangeExtension(url, ".webp");
            }
            else if ((extension == ".jpg" || extension == ".jpeg") && imageOptions.RecodeJpegToWebp)
            {
                return Path.ChangeExtension(url, ".webp");
            }

            return url;
        }

        public static void ApplyStyles(IReadOnlyBuildConfig config,
                                       MarkdownDocument document)
        {
            if (config == null)
                throw new InvalidOperationException("Settings not configured");

            foreach (MarkdownObject? node in document.Descendants())
            {
                if (node is HeadingBlock heading)
                {
                    switch (heading.Level)
                    {
                        case 1:
                            AddStyleClass(node, config.StyleClasses.Heading1);
                            break;
                        case 2:
                            AddStyleClass(node, config.StyleClasses.Heading2);
                            break;
                        case 3:
                            AddStyleClass(node, config.StyleClasses.Heading3);
                            break;
                    }
                }
                else if (node is Block)
                {
                    if (node is Markdig.Extensions.Tables.Table)
                        AddStyleClass(node, config.StyleClasses.Table);
                    else if (node is QuoteBlock)
                        AddStyleClass(node, config.StyleClasses.Blockquote);
                    else if (node is Markdig.Extensions.Figures.Figure)
                        AddStyleClass(node, config.StyleClasses.Figure);
                    else if (node is Markdig.Extensions.Figures.FigureCaption)
                        AddStyleClass(node, config.StyleClasses.FigureCaption);
                }
                else if (node is LinkInline link)
                {
                    if (link.IsImage)
                    {
                        AddStyleClass(link, config.StyleClasses.Image);
                    }
                    else
                    {
                        AddStyleClass(node, config.StyleClasses.Link);
                    }
                }
                else if (node is ListBlock listBlock)
                {
                    if (listBlock.IsOrdered)
                        AddStyleClass(node, config.StyleClasses.OrderedList);
                    else
                        AddStyleClass(node, config.StyleClasses.UnorederedList);
                }
                else if (node is ListItemBlock)
                {
                    AddStyleClass(node, config.StyleClasses.ListItem);
                }
            }
        }
    }
}
