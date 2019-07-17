//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace BookGen.Core.Markdown.Pipeline
{
    internal class EpubModifier: IMarkdownExtension
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
            foreach (var node in document.Descendants())
            {
                if (node is HeadingBlock heading)
                {
                    ++heading.Level;
                }
                else if (node is LinkInline link && link.IsImage)
                {
                    var inlinekey = PipelineHelpers.ToImgCacheKey(link.Url, RuntimeConfig);
                    if (RuntimeConfig.InlineImgCache.ContainsKey(inlinekey))
                    {
                        link.Url = RuntimeConfig.InlineImgCache[inlinekey];
                    }
                }
            }
        }
    }
}
