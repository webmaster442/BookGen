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
        public static StyleClasses Styles { get; set; }

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
            if (Styles == null)
                throw new InvalidOperationException("Styles not configured");

            foreach (var node in document.Descendants())
            {
                if (node is HeadingBlock heading)
                {
                    switch (heading.Level)
                    {
                        case 1:
                            AddStyleClass(node, Styles.Heading1);
                            break;
                        case 2:
                            AddStyleClass(node, Styles.Heading2);
                            break;
                        case 3:
                            AddStyleClass(node, Styles.Heading3);
                            break;
                    }
                }
                else if (node is Block)
                {
                    if (node is Markdig.Extensions.Tables.Table)
                        AddStyleClass(node, Styles.Table);
                    else if (node is QuoteBlock)
                        AddStyleClass(node, Styles.Blockquote);
                    else if (node is Markdig.Extensions.Figures.Figure)
                        AddStyleClass(node, Styles.Figure);
                    else if (node is Markdig.Extensions.Figures.FigureCaption)
                        AddStyleClass(node, Styles.FigureCaption);
                }
                else if (node is LinkInline link)
                {
                    if (link.IsImage)
                        AddStyleClass(node, Styles.Image);
                    else
                        AddStyleClass(node, Styles.Link);
                }
                else if (node is ListBlock listBlock)
                {
                    if (listBlock.IsOrdered)
                        AddStyleClass(node, Styles.OrderedList);
                    else
                        AddStyleClass(node, Styles.UnorederedList);
                }
                else if (node is ListItemBlock)
                    AddStyleClass(node, Styles.ListItem);
            }
        }
    }
}
