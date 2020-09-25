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

        public IReadonlyRuntimeSettings? RuntimeConfig { get; set; }

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
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            _pipeline = pipeline;
            _pipeline.DocumentProcessed += PipelineOnDocumentProcessed;
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
