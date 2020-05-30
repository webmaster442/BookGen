//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;

namespace BookGen.Core.Markdown.Pipeline
{
    public class WordpressModifier: IMarkdownExtension
    {
        public static IReadonlyRuntimeSettings? RuntimeConfig { get; set; }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            pipeline.DocumentProcessed -= PipelineOnDocumentProcessed;
            pipeline.DocumentProcessed += PipelineOnDocumentProcessed;
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
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
