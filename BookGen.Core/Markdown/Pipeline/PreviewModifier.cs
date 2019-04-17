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
using System.Web;

namespace BookGen.Core.Markdown.Pipeline
{
    internal class PreviewModifier : IMarkdownExtension
    {
        public static FsPath PreviewFilePath { get; set; }

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
            if (PreviewFilePath != null)
            {
                foreach (var node in document.Descendants())
                {
                    if (node is LinkInline link)
                    {
                        if (link.IsImage)
                        {
                            if (!link.Url.StartsWith("http://") && !link.Url.StartsWith("https://"))
                            {
                                var file = new FsPath(link.Url).GetAbsolutePathRelativeTo(PreviewFilePath).ToString();
                                byte[] contents = File.ReadAllBytes(file);
                                var mime = MimeMapping.GetMimeMapping(file);
                                link.Url = $"data:{mime};base64,{Convert.ToBase64String(contents)}";
                            }
                        }
                    }
                }
            }
        }
    }
}
