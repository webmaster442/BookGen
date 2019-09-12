//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.IO;

namespace BookGen.Core.Markdown.Pipeline
{
    internal class PreviewModifier : IMarkdownExtension
    {
        public static FsPath WorkDir { get; set; }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            pipeline.DocumentProcessed -= PipelineOnDocumentProcessed;
            pipeline.DocumentProcessed += PipelineOnDocumentProcessed;
        }
        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            PipelineHelpers.SetupSyntaxRender(renderer);
        }

        private void PipelineOnDocumentProcessed(MarkdownDocument document)
        {
            foreach (var node in document.Descendants())
            {
                if (node is LinkInline link && link.IsImage)
                {
                    link.Url = Base64EncodeIfLocal(link.Url);
                }
            }
        }

        private string Base64EncodeIfLocal(string url)
        {
            if (url.StartsWith("https://") || url.StartsWith("http://"))
                return url;

            var inlinePath = new FsPath(url).GetAbsolutePathRelativeTo(WorkDir).ToString();

            byte[] contents = File.ReadAllBytes(inlinePath);

            string mime = "application/octet-stream";

            switch (Path.GetExtension(inlinePath))
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
