//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;

namespace BookGen.Core.Markdown.Pipeline
{
    internal class WebModifier : IMarkdownExtension
    {
        public static IReadonlyRuntimeSettings RuntimeConfig { get; set; }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            pipeline.DocumentProcessed -= PipelineOnDocumentProcessed;
            pipeline.DocumentProcessed += PipelineOnDocumentProcessed;
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
        }

        private static void AddStyleClass(MarkdownObject node, string style)
        {
            if (string.IsNullOrEmpty(style)) return;
            node.GetAttributes().AddClass(style);
        }

        private static bool IsOffHostLink(LinkInline link)
        {
            return !link.Url.StartsWith(RuntimeConfig.Configruation.HostName);
        }

        private static void PipelineOnDocumentProcessed(MarkdownDocument document)
        {
            if (RuntimeConfig == null)
                throw new InvalidOperationException("Settings not configured");

            foreach (var node in document.Descendants())
            {
                if (node is HeadingBlock heading)
                {
                    switch (heading.Level)
                    {
                        case 1:
                            AddStyleClass(node, RuntimeConfig.Configruation.StyleClasses.Heading1);
                            break;
                        case 2:
                            AddStyleClass(node, RuntimeConfig.Configruation.StyleClasses.Heading2);
                            break;
                        case 3:
                            AddStyleClass(node, RuntimeConfig.Configruation.StyleClasses.Heading3);
                            break;
                    }
                }
                else if (node is Block)
                {
                    if (node is Markdig.Extensions.Tables.Table)
                        AddStyleClass(node, RuntimeConfig.Configruation.StyleClasses.Table);
                    else if (node is QuoteBlock)
                        AddStyleClass(node, RuntimeConfig.Configruation.StyleClasses.Blockquote);
                    else if (node is Markdig.Extensions.Figures.Figure)
                        AddStyleClass(node, RuntimeConfig.Configruation.StyleClasses.Figure);
                    else if (node is Markdig.Extensions.Figures.FigureCaption)
                        AddStyleClass(node, RuntimeConfig.Configruation.StyleClasses.FigureCaption);
                }
                else if (node is LinkInline link)
                {
                    if (link.IsImage)
                    {

                        var inlinekey = PipelineHelpers.ToImgCacheKey(link.Url, RuntimeConfig);
                        if (RuntimeConfig.InlineImgCache.ContainsKey(inlinekey))
                        {
                            link.Url = RuntimeConfig.InlineImgCache[inlinekey];
                        }

                        AddStyleClass(link, RuntimeConfig.Configruation.StyleClasses.Image);
                    }
                    else
                    {
                        if (IsOffHostLink(link) && RuntimeConfig.Configruation.LinksOutSideOfHostOpenNewTab)
                        {
                            link.GetAttributes().AddProperty("target", "_blank");
                        }
                        else
                            AddStyleClass(node, RuntimeConfig.Configruation.StyleClasses.Link);
                    }
                }
                else if (node is ListBlock listBlock)
                {
                    if (listBlock.IsOrdered)
                        AddStyleClass(node, RuntimeConfig.Configruation.StyleClasses.OrderedList);
                    else
                        AddStyleClass(node, RuntimeConfig.Configruation.StyleClasses.UnorederedList);
                }
                else if (node is ListItemBlock)
                    AddStyleClass(node, RuntimeConfig.Configruation.StyleClasses.ListItem);
            }
        }
    }

}
