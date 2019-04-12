//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookGen.Framework
{
    public class MarkdownModifier : IMarkdownExtension
    {
        public static RuntimeSettings Settings { get; set; }

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
            return !link.Url.StartsWith(Settings.Configruation.HostName);
        }

        private static string ToImgCacheKey(string s)
        {
            Uri baseUri = new Uri(Settings.Configruation.HostName);
            Uri full = new Uri(baseUri, s);
            string fsPath = full.ToString().Replace(Settings.Configruation.HostName, Settings.SourceDirectory.ToString()+"\\");
            return fsPath.Replace("/", "\\");
        }

        private static void PipelineOnDocumentProcessed(MarkdownDocument document)
        {
            if (Settings == null)
                throw new InvalidOperationException("Settings not configured");

            foreach (var node in document.Descendants())
            {
                if (node is HeadingBlock heading)
                {
                    switch (heading.Level)
                    {
                        case 1:
                            AddStyleClass(node, Settings.Configruation.StyleClasses.Heading1);
                            break;
                        case 2:
                            AddStyleClass(node, Settings.Configruation.StyleClasses.Heading2);
                            break;
                        case 3:
                            AddStyleClass(node, Settings.Configruation.StyleClasses.Heading3);
                            break;
                    }
                }
                else if (node is Block)
                {
                    if (node is Markdig.Extensions.Tables.Table)
                        AddStyleClass(node, Settings.Configruation.StyleClasses.Table);
                    else if (node is QuoteBlock)
                        AddStyleClass(node, Settings.Configruation.StyleClasses.Blockquote);
                    else if (node is Markdig.Extensions.Figures.Figure)
                        AddStyleClass(node, Settings.Configruation.StyleClasses.Figure);
                    else if (node is Markdig.Extensions.Figures.FigureCaption)
                        AddStyleClass(node, Settings.Configruation.StyleClasses.FigureCaption);
                }
                else if (node is LinkInline link)
                {
                    if (link.IsImage)
                    {

                        var inlinekey = ToImgCacheKey(link.Url);
                        if (Settings.InlineImgCache.ContainsKey(inlinekey))
                        {
                            link.Url = Settings.InlineImgCache[inlinekey];
                        }

                        AddStyleClass(link, Settings.Configruation.StyleClasses.Image);
                    }
                    else
                    {
                        if (IsOffHostLink(link) && Settings.Configruation.LinksOutSideOfHostOpenNewTab)
                        {
                            link.GetAttributes().AddProperty("target", "_blank");
                        }
                        else
                            AddStyleClass(node, Settings.Configruation.StyleClasses.Link);
                    }
                }
                else if (node is ListBlock listBlock)
                {
                    if (listBlock.IsOrdered)
                        AddStyleClass(node, Settings.Configruation.StyleClasses.OrderedList);
                    else
                        AddStyleClass(node, Settings.Configruation.StyleClasses.UnorederedList);
                }
                else if (node is ListItemBlock)
                    AddStyleClass(node, Settings.Configruation.StyleClasses.ListItem);
            }
        }
    }
}
