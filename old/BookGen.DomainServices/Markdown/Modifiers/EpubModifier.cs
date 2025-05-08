//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices.Markdown.Renderers;
using BookGen.Interfaces;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;

namespace BookGen.DomainServices.Markdown.Modifiers
{
    internal sealed class EpubModifier : IMarkdownExtensionWithRuntimeConfig, IMarkdownExtensionWithSyntaxToggle, IDisposable
    {
        public IReadonlyRuntimeSettings? RuntimeConfig { get; set; }
        private MarkdownPipelineBuilder? _pipeline;
        private JavaScriptInterop? _interop;

        public EpubModifier()
        {
            _interop = new JavaScriptInterop();
        }

        public bool SyntaxEnabled
        {
            get { return SyntaxRenderer.Enabled; }
            set { SyntaxRenderer.Enabled = value; }
        }

        public void Dispose()
        {
            if (_interop != null)
            {
                _interop.Dispose();
                _interop = null;
            }
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
            if (_interop == null)
                throw new InvalidOperationException();

            PipelineHelpers.SetupSyntaxRenderForPreRender(renderer, _interop);
        }

        private void PipelineOnDocumentProcessed(MarkdownDocument document)
        {
            if (RuntimeConfig == null)
                return;

            if (SyntaxEnabled)
                PipelineHelpers.AppendPrismCss(document);

            PipelineHelpers.ApplyStyles(RuntimeConfig.Configuration.TargetEpub,
                                        document);

            PipelineHelpers.RenderImages(RuntimeConfig, document);

            foreach (MarkdownObject? node in document.Descendants())
            {
                if (node is HeadingBlock heading)
                {
                    ++heading.Level;
                }
            }
        }
    }
}
