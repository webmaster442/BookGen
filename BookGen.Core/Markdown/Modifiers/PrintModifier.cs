//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using BookGen.Core.Markdown.Renderers;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Linq;

namespace BookGen.Core.Markdown.Modifiers
{
    internal sealed class PrintModifier : IMarkdownExtensionWithRuntimeConfig, IMarkdownExtensionWithSyntaxToggle, IDisposable
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

            PipelineHelpers.SetupSyntaxRender(renderer, _interop);
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

        private string RewiteToHostUrl(string? url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            var parts = url.Replace("\\", "/").Split('/').ToList();
            var imgdirIndex = parts.IndexOf(RuntimeConfig!.Configuration.ImageDir);

            return string.Join("/", parts.ToArray(), imgdirIndex, parts.Count - imgdirIndex);
        }
    }
}
