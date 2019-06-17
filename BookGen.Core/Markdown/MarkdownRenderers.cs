//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Core.Markdown.Pipeline;
using Markdig;

namespace BookGen.Core.Markdown
{
    public static class MarkdownRenderers
    {
        private static readonly MarkdownPipeline _webpipeline;
        private static readonly MarkdownPipeline _printpipeline;
        private static readonly MarkdownPipeline _plainpipeline;
        private static readonly MarkdownPipeline _epubpipeline;

        static MarkdownRenderers()
        {
            _plainpipeline = new MarkdownPipelineBuilder().Build();
            _webpipeline = new MarkdownPipelineBuilder().Use<WebModifier>().UseAdvancedExtensions().Build();
            _printpipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Use<PrintModifier>().Build();
            _epubpipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Use<EpubModifier>().Build();
        }

        /// <summary>
        /// Generate markdown to html
        /// </summary>
        /// <param name="md">Markdown input string</param>
        /// <returns>html page</returns>
        public static string Markdown2WebHTML(string md, IReadonlyRuntimeSettings settings)
        {
            WebModifier.RuntimeConfig = settings;
            return Markdig.Markdown.ToHtml(md, _webpipeline);
        }

        public static string Markdown2EpubHtml(string md, IReadonlyRuntimeSettings settings)
        {
            EpubModifier.RuntimeConfig = settings;
            return Markdig.Markdown.ToHtml(md, _epubpipeline);
        }

        /// <summary>
        /// Generate markdown to plain text
        /// </summary>
        /// <param name="md">Markdown input string</param>
        /// <returns>plain text</returns>
        public static string Markdown2Plain(string md)
        {
            return Markdig.Markdown.ToPlainText(md, _plainpipeline);
        }

        public static string Markdown2PrintHTML(string md, Config configuration)
        {
            PrintModifier.Configuration = configuration;
            return Markdig.Markdown.ToHtml(md, _printpipeline);
        }
    }
}
