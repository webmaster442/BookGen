//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Linq;

namespace BookGen.Core.Markdown.Modifiers
{
    internal class PrintModifier : IBookGenMarkdownExtension
    {
        public IReadonlyRuntimeSettings? RuntimeConfig { get; set; }

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
            if (RuntimeConfig == null)
                return;

            PipelineHelpers.ApplyStyles(RuntimeConfig.Configuration.TargetPrint, document);
            PipelineHelpers.RenderImages(RuntimeConfig, document);

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
            var imgdirIndex = parts.IndexOf(RuntimeConfig!.Configuration.ImageDir);

            return string.Join("/", parts.ToArray(), imgdirIndex, parts.Count - imgdirIndex);
        }
    }
}
