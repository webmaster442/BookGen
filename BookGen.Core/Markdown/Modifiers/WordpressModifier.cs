//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using System;

namespace BookGen.Core.Markdown.Modifiers
{
    public sealed class WordpressModifier: IMarkdownExtensionWithRuntimeConfig, IDisposable
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

        private void PipelineOnDocumentProcessed(MarkdownDocument document)
        {
            if (RuntimeConfig == null)
                return;

            bool success = RuntimeConfig
                           .CurrentBuildConfig
                           .TemplateOptions
                           .TryGetOption(TemplateOptions.WordpressSkipPageTitle, out bool skipheader);

            if (success && skipheader)
            {
                PipelineHelpers.DeleteFirstH1(document);
            }

            PipelineHelpers.ApplyStyles(RuntimeConfig.Configuration.TargetEpub,
                                        document);

            PipelineHelpers.RenderImages(RuntimeConfig, document);
        }
    }
}
