//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices.Markdown.Renderers;
using BookGen.Interfaces;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace BookGen.DomainServices.Markdown.Modifiers
{
    internal sealed class PrintModifier : 
        IMarkdownExtensionWithRuntimeConfig,
        IMarkdownExtensionWithSyntaxToggle,
        IMarkdownExtensionWithSvgPassthoughToggle,
        IDisposable
    {
        private MarkdownPipelineBuilder? _pipeline;
        private JavaScriptInterop? _interop;

        public IReadonlyRuntimeSettings? RuntimeConfig { get; set; }

        public PrintModifier()
        {
            _interop = new JavaScriptInterop();
        }

        public bool SyntaxEnabled
        {
            get { return SyntaxRenderer.Enabled; }
            set { SyntaxRenderer.Enabled = value; }
        }

        public bool SvgPasstrough { get; set; }

        public void Dispose()
        {
            if (_pipeline != null)
            {
                _pipeline.DocumentProcessed -= PipelineOnDocumentProcessed;
                _pipeline = null;
            }
            if (_interop != null)
            {
                _interop.Dispose();
                _interop = null;
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
            PipelineHelpers.SetupLinkInlineRendererWithSvgSupport(renderer);
        }

        private void PipelineOnDocumentProcessed(MarkdownDocument document)
        {
            if (RuntimeConfig == null)
                return;

            if (SyntaxEnabled)
                PipelineHelpers.AppendPrismCss(document, isPrinting: true);

            PipelineHelpers.ApplyStyles(RuntimeConfig.Configuration.TargetPrint, document);
            PipelineHelpers.RenderImages(RuntimeConfig, document);


            foreach (MarkdownObject? node in document.Descendants())
            {
                if (node is HeadingBlock heading)
                {
                    ++heading.Level;
                }
                else if (node is LinkInline link 
                    && link.IsImage 
                    && !string.IsNullOrEmpty(link.Url))
                {
                    if (!SvgPasstrough)
                    {
                        link.Url = RewiteToHostUrl(link.Url);
                    }
                }
            }
        }

        private string RewiteToHostUrl(string? url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            if (url.StartsWith("data:"))
                return url;

            var parts = url.ToLower().Replace("\\", "/").Split('/').ToList();
            int imgdirIndex = parts.IndexOf(RuntimeConfig!.Configuration.ImageDir.ToLower());

            return string.Join("/", parts.ToArray(), imgdirIndex, parts.Count - imgdirIndex);
        }
    }
}
