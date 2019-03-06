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

namespace BookGen.Framework
{
    public class MarkdownModifier : IMarkdownExtension
    {
        public static Config Config { get; set; }

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

        private static void PipelineOnDocumentProcessed(MarkdownDocument document)
        {
            if (Config == null)
                throw new InvalidOperationException("Config not configured");

            foreach (var node in document.Descendants())
            {
                if (node is HeadingBlock heading)
                {
                    switch (heading.Level)
                    {
                        case 1:
                            AddStyleClass(node, Config.StyleClasses.Heading1);
                            break;
                        case 2:
                            AddStyleClass(node, Config.StyleClasses.Heading2);
                            break;
                        case 3:
                            AddStyleClass(node, Config.StyleClasses.Heading3);
                            break;
                    }
                }
                else if (node is Block)
                {
                    if (node is Markdig.Extensions.Tables.Table)
                        AddStyleClass(node, Config.StyleClasses.Table);
                    else if (node is QuoteBlock)
                        AddStyleClass(node, Config.StyleClasses.Blockquote);
                    else if (node is Markdig.Extensions.Figures.Figure)
                        AddStyleClass(node, Config.StyleClasses.Figure);
                    else if (node is Markdig.Extensions.Figures.FigureCaption)
                        AddStyleClass(node, Config.StyleClasses.FigureCaption);
                }
                else if (node is LinkInline link)
                {
                    if (link.IsImage)
                        AddStyleClass(node, Config.StyleClasses.Image);
                    else
                        AddStyleClass(node, Config.StyleClasses.Link);
                }
                else if (node is ListBlock listBlock)
                {
                    if (listBlock.IsOrdered)
                        AddStyleClass(node, Config.StyleClasses.OrderedList);
                    else
                        AddStyleClass(node, Config.StyleClasses.UnorederedList);
                }
                else if (node is ListItemBlock)
                    AddStyleClass(node, Config.StyleClasses.ListItem);
            }
        }
    }
}
