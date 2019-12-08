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
        public static IReadonlyRuntimeSettings? RuntimeConfig { get; set; }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            pipeline.DocumentProcessed -= PipelineOnDocumentProcessed;
            pipeline.DocumentProcessed += PipelineOnDocumentProcessed;
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            // Method intentionally left empty.
        }

        private static bool IsOffHostLink(LinkInline link)
        {
            return !link.Url.StartsWith(RuntimeConfig?.Configuration.HostName);
        }

        private static void PipelineOnDocumentProcessed(MarkdownDocument document)
        {
            if (RuntimeConfig == null)
                throw new InvalidOperationException("Settings not configured");

            PipelineHelpers.ApplyStyles(RuntimeConfig.Configuration.TargetWeb,
                                        document);

            foreach (var node in document.Descendants())
            {
                if (node is LinkInline link)
                {
                    if (link.IsImage && RuntimeConfig.InlineImgCache?.Count > 0)
                    {
                        var inlinekey = PipelineHelpers.ToImgCacheKey(link.Url, RuntimeConfig.OutputDirectory);
                        if (RuntimeConfig.InlineImgCache.ContainsKey(inlinekey))
                        {
                            link.Url = RuntimeConfig.InlineImgCache[inlinekey];
                        }
                    }
                    else if (IsOffHostLink(link) && RuntimeConfig.Configuration.LinksOutSideOfHostOpenNewTab)
                    {
                        link.GetAttributes().AddProperty("target", "_blank");
                    }
                }
            }
        }
    }
}
