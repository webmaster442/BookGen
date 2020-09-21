//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;

namespace BookGen.Core.Markdown
{
    public static class MarkdownRenderers
    {
        /// <summary>
        /// Generate Markdown to html with different pipelines
        /// </summary>
        /// <param name="markdown">Markdown to render</param>
        /// <param name="pipeline">Pipeline to use</param>
        /// <param name="settings">Runtime settings</param>
        /// <returns>Html text</returns>
        public static string RenderMarkdown(string markdown, BookGenPipeline pipeline, IReadonlyRuntimeSettings settings)
        {
            pipeline.InjectRuntimeConfig(settings);
            return Markdig.Markdown.ToHtml(markdown, pipeline.MarkdownPipeline);
        }
    }
}
