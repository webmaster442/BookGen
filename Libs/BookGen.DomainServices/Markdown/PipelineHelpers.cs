//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api.Configuration;
using BookGen.Interfaces;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Text;

namespace BookGen.DomainServices.Markdown
{
    internal static class PipelineHelpers
    {
        public static void AppendPrismCss(MarkdownDocument document)
        {
            var content = new StringBuilder();
            content.Append("<style type=\"text/css\">\r\n");
            content.Append(Resources.ResourceHandler.GetFile(Resources.KnownFile.PrismCss));
            content.Append("</style>\r\n");
            var block = new HtmlBlock(new HtmlBlockParser());
            block.Lines = new Markdig.Helpers.StringLineGroup(content.ToString());
            document.Insert(0, block);
        }

        public static void SetupSyntaxRender(IMarkdownRenderer renderer, JavaScriptInterop interop)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            if (renderer is not TextRendererBase<HtmlRenderer> htmlRenderer) return;

            CodeBlockRenderer? originalCodeBlockRenderer = htmlRenderer.ObjectRenderers.FindExact<CodeBlockRenderer>();
            if (originalCodeBlockRenderer != null)
            {
                htmlRenderer.ObjectRenderers.Remove(originalCodeBlockRenderer);
                htmlRenderer.ObjectRenderers.AddIfNotAlready(new Renderers.SyntaxRenderer(originalCodeBlockRenderer, interop));
            }
        }

        public static string ToImgCacheKey(string url, FsPath outputDir)
        {
            var requested = new FsPath(url);
            return requested.GetAbsolutePathRelativeTo(outputDir).ToString();
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

        public static void RenderImages(IReadonlyRuntimeSettings RuntimeConfig,
                                        MarkdownDocument document)
        {
            foreach (MarkdownObject? node in document.Descendants())
            {
                if (node is LinkInline link && link.IsImage)
                {
                    link.Url = FixExtension(link.Url, RuntimeConfig.CurrentBuildConfig.ImageOptions.RecodeJpegToWebp);
                    if (RuntimeConfig.InlineImgCache?.Count > 0)
                    {
                        string? inlinekey = ToImgCacheKey(link.Url, RuntimeConfig.OutputDirectory);
                        if (RuntimeConfig.InlineImgCache.ContainsKey(inlinekey))
                        {
                            link.Url = RuntimeConfig.InlineImgCache[inlinekey];
                        }
                    }
                }
            }
        }

        private static string FixExtension(string? url, bool jpegtoWebp)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            string extension = Path.GetExtension(url);
            if (extension == ".svg")
            {
                return Path.ChangeExtension(url, ".png");
            }
            else if ((extension == ".jpg" || extension == ".jpeg") && jpegtoWebp)
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
