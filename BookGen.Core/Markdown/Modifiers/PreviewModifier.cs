//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using BookGen.Core.Markdown.Renderers;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using SkiaSharp;
using Svg.Skia;
using System;

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

            if (Path is null)
            {
                inlinePath = new FsPath(url);
            }
            else
            {
                inlinePath = new FsPath(url).GetAbsolutePathRelativeTo(Path);
            }

            if (!inlinePath.IsExisting)
            {
                return string.Empty;
            }

            byte[] data = CompressWebp(inlinePath);

            return $"data:image/webp;base64,{Convert.ToBase64String(data)}";

        }

        private static byte[] CompressWebp(FsPath inlinePath)
        {
            if (inlinePath.Extension == ".svg")
            {
                using (SKBitmap renderedSvg = RenderSvg(inlinePath))
                {
                    using var data = renderedSvg.Encode(SKEncodedImageFormat.Webp, 80);
                    return data.ToArray();
                }
                
            }
            using (SKBitmap bmp = SKBitmap.Decode(inlinePath.Filename))
            {
                using var data = bmp.Encode(SKEncodedImageFormat.Webp, 80);
                return data.ToArray();
            }
        }

        private static SKBitmap RenderSvg(FsPath inlinePath)
        {
            var svg = new SKSvg();
            svg.Load(inlinePath.ToString());

            if (svg.Picture == null)
                return new SKBitmap(1, 1, false);

            SKRect svgSize = svg.Picture.CullRect;

            SKBitmap result = new SKBitmap((int)svgSize.Width, (int)svgSize.Height, false);

            using (SKCanvas canvas = new SKCanvas(result))
            {
                canvas.DrawPicture(svg.Picture);
                canvas.Flush();
            }

            return result;
        }
    }
}