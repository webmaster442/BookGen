﻿//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using BookGen.Core.Markdown.Renderers;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.IO;

namespace BookGen.Core.Markdown.Modifiers
{
    internal sealed class PreviewModifier : IMarkdownExtensionWithPath, IMarkdownExtensionWithSyntaxToggle, IDisposable
    {
        private MarkdownPipelineBuilder? _pipeline;
        private JavaScriptInterop? _interop;

        public PreviewModifier()
        {
            Path = FsPath.Empty;
            _interop = new JavaScriptInterop();
        }

        public FsPath Path { get; set; }

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
            if (SyntaxEnabled)
                PipelineHelpers.AppendPrismCss(document);

            foreach (var node in document.Descendants())
            {
                if (node is LinkInline link
                    && link.IsImage
                    && !string.IsNullOrEmpty(link.Url))
               {
                    link.Url = Base64EncodeIfLocal(link.Url);
                }
            }
        }

        private string Base64EncodeIfLocal(string url)
        {
            if (url.StartsWith("https://") || url.StartsWith("http://"))
                return url;

            FsPath inlinePath;

            if (object.ReferenceEquals(Path, null))
            {
                inlinePath = new FsPath(url);
            }
            else
            {
                inlinePath = new FsPath(url).GetAbsolutePathRelativeTo(Path!);
            }

            if (!inlinePath.IsExisting)
            {
                return string.Empty;
            }

            byte[] contents = File.ReadAllBytes(inlinePath.ToString());

            string mime = "application/octet-stream";

            switch (System.IO.Path.GetExtension(inlinePath.ToString()))
            {
                case ".jpg":
                case ".jpeg":
                    mime = "image/jpeg";
                    break;
                case ".png":
                    mime = "image/png";
                    break;
                case ".gif":
                    mime = "image/gif";
                    break;
                case ".webp":
                    mime = "image/webp";
                    break;
            }
            return $"data:{mime};base64,{Convert.ToBase64String(contents)}";
        }
    }
}
