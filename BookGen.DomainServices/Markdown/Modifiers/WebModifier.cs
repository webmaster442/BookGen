//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;

namespace BookGen.Core.Markdown.Modifiers
{
    internal sealed class WebModifier : IMarkdownExtensionWithRuntimeConfig, IDisposable
    {
        private MarkdownPipelineBuilder? _pipeline;

        public IReadonlyRuntimeSettings? RuntimeConfig { get; set; }

        public void Dispose()
        {
            if (_pipeline != null)
            {
                _pipeline.DocumentProcessed -= PipelineOnDocumentProcessed;
                _pipeline = null;
            }
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            _pipeline = pipeline;
            _pipeline.DocumentProcessed += PipelineOnDocumentProcessed;
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            // Method intentionally left empty.
        }

        private static bool IsOffHostLink(LinkInline link, IReadonlyRuntimeSettings RuntimeConfig)
        {
            if (RuntimeConfig.Configuration != null)
                return !link.Url?.StartsWith(RuntimeConfig.Configuration.HostName) ?? false;
            else
                return true;
        }

        private void PipelineOnDocumentProcessed(MarkdownDocument document)
        {
            if (RuntimeConfig == null)
                throw new InvalidOperationException("Settings not configured");

            PipelineHelpers.ApplyStyles(RuntimeConfig.Configuration.TargetWeb,
                                        document);

            PipelineHelpers.RenderImages(RuntimeConfig,
                                         document);
            foreach (var node in document.Descendants())
            {
                if (node is LinkInline link
                    && IsOffHostLink(link, RuntimeConfig)
                    && RuntimeConfig.Configuration.LinksOutSideOfHostOpenNewTab)
                {
                    link.GetAttributes().AddProperty("target", "_blank");
                }
            }
        }
    }
}
