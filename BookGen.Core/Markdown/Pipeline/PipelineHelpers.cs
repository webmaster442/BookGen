//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;

namespace BookGen.Core.Markdown.Pipeline
{
    internal static class PipelineHelpers
    {
        public static void SetupSyntaxRender(IMarkdownRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            if (!(renderer is TextRendererBase<HtmlRenderer> htmlRenderer)) return;

            var originalCodeBlockRenderer = htmlRenderer.ObjectRenderers.FindExact<CodeBlockRenderer>();
            if (originalCodeBlockRenderer != null)
                htmlRenderer.ObjectRenderers.Remove(originalCodeBlockRenderer);

            htmlRenderer.ObjectRenderers.AddIfNotAlready(new SyntaxRenderer(originalCodeBlockRenderer));
        }

        public static string ToImgCacheKey(string url, FsPath outputDir)
        {
            FsPath requested = new FsPath(url);
            return requested.GetAbsolutePathRelativeTo(outputDir).ToString();
        }

        private static void AddStyleClass(MarkdownObject node, string style)
        {
            if (string.IsNullOrEmpty(style)) return;
            node.GetAttributes().AddClass(style);
        }

        public static void ApplyStyles(BuildConfig config,
                                       MarkdownDocument document)
        {
            if (config == null)
                throw new InvalidOperationException("Settings not configured");

            foreach (var node in document.Descendants())
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
