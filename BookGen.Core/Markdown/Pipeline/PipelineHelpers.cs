//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using System;

namespace BookGen.Core.Markdown.Pipeline
{
    internal static class PipelineHelpers
    {
        public static void SetupSyntaxRender(IMarkdownRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            if (!(renderer is TextRendererBase<HtmlRenderer> htmlRenderer)) return;

            var originalCodeBlockRenderer = htmlRenderer.ObjectRenderers.FindExact<CodeBlockRenderer>();
            if (originalCodeBlockRenderer != null)
                htmlRenderer.ObjectRenderers.Remove(originalCodeBlockRenderer);


            htmlRenderer.ObjectRenderers.AddIfNotAlready(new SyntaxRenderer(originalCodeBlockRenderer));
        }

        public static string ToImgCacheKey(string url, IReadonlyRuntimeSettings RuntimeConfig)
        {
            Uri baseUri = new Uri(RuntimeConfig.Configruation.HostName);
            Uri full = new Uri(baseUri, url);
            string fsPath = full.ToString().Replace(RuntimeConfig.Configruation.HostName, RuntimeConfig.SourceDirectory.ToString() + "\\");
            return fsPath.Replace("/", "\\");
        }
    }
}
