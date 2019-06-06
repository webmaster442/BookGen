//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Linq;

namespace BookGen.Core.Markdown.Pipeline
{
    internal class PrintModifier : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            pipeline.DocumentProcessed -= PipelineOnDocumentProcessed;
            pipeline.DocumentProcessed += PipelineOnDocumentProcessed;
        }

        public static Config Configuration { get; set; }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            PipelineHelpers.SetupSyntaxRender(renderer);
        }

        private void PipelineOnDocumentProcessed(MarkdownDocument document)
        {
            foreach (var node in document.Descendants())
            {
                if (node is HeadingBlock heading)
                {
                    ++heading.Level;
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
            var parts = url.Replace("\\", "/").Split('/').ToList();
            var imgdirIndex = parts.IndexOf(Configuration.ImageDir);

            return string.Join("/", parts.ToArray(), imgdirIndex, parts.Count - imgdirIndex);
        }
    }
}
