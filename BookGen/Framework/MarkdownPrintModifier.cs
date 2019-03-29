//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Configuration;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Linq;

namespace BookGen.Framework
{
    public class MarkdownPrintModifier : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            pipeline.DocumentProcessed -= PipelineOnDocumentProcessed;
            pipeline.DocumentProcessed += PipelineOnDocumentProcessed;
        }

        public static Config Configuration { get; set; }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            if (!(renderer is TextRendererBase<HtmlRenderer> htmlRenderer)) return;

            var originalCodeBlockRenderer = htmlRenderer.ObjectRenderers.FindExact<CodeBlockRenderer>();
            if (originalCodeBlockRenderer != null)
                htmlRenderer.ObjectRenderers.Remove(originalCodeBlockRenderer);


            htmlRenderer.ObjectRenderers.AddIfNotAlready(new PrintSyntaxHiglighter(originalCodeBlockRenderer));
        }

        private void PipelineOnDocumentProcessed(MarkdownDocument document)
        {
            foreach (var node in document.Descendants())
            {
                if (node is HeadingBlock heading)
                {
                    heading.Level += 1;
                }
                else if (node is LinkInline link)
                {
                    if (link.IsImage)
                        link.Url = RewiteToHostUrl(link.Url);
                }
            }
        }

        private string RewiteToHostUrl(string url)
        {
            var imgDir = Configuration.ImageDir.ToPath();

            var parts = url.Split('/').ToList();
            var imgdirIndex = parts.IndexOf(Configuration.ImageDir);

            return string.Join("/", parts.ToArray(), imgdirIndex, parts.Count - imgdirIndex);
        }
    }
}
