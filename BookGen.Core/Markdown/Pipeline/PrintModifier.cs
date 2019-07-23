//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Linq;

namespace BookGen.Core.Markdown.Pipeline
{
    internal class PrintModifier : IMarkdownExtension
    {
        public static IReadonlyRuntimeSettings RuntimeConfig { get; set; }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            pipeline.DocumentProcessed -= PipelineOnDocumentProcessed;
            pipeline.DocumentProcessed += PipelineOnDocumentProcessed;
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            PipelineHelpers.SetupSyntaxRender(renderer);
        }

        private void PipelineOnDocumentProcessed(MarkdownDocument document)
        {
            PipelineHelpers.ApplyStyles(RuntimeConfig.Configruation.TargetPrint, document);

            foreach (var node in document.Descendants())
            {
                if (node is HeadingBlock heading)
                {
                    ++heading.Level;
                }
                else if (node is LinkInline link && link.IsImage)
                {
                    link.Url = RewiteToHostUrl(link.Url);
                }
            }
        }

        private string RewiteToHostUrl(string url)
        {
            var parts = url.Replace("\\", "/").Split('/').ToList();
            var imgdirIndex = parts.IndexOf(RuntimeConfig.Configruation.ImageDir);

            return string.Join("/", parts.ToArray(), imgdirIndex, parts.Count - imgdirIndex);
        }
    }
}
