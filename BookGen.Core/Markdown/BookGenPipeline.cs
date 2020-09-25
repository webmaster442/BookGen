//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using BookGen.Core.Markdown.Modifiers;
using Markdig;
using System;

namespace BookGen.Core.Markdown
{
    public sealed class BookGenPipeline : IDisposable
    {
        public static MarkdownPipeline Web
        {
            get => new MarkdownPipelineBuilder().Use<WebModifier>().UseAdvancedExtensions().Build();
        }
        
        public static MarkdownPipeline Print
        {
            get => new MarkdownPipelineBuilder().UseAdvancedExtensions().Use<PrintModifier>().Build();
        }
        
        public static MarkdownPipeline Plain
        {
            get => new MarkdownPipelineBuilder().Build();
        }

        public static MarkdownPipeline Epub
        {
            get => new MarkdownPipelineBuilder().UseAdvancedExtensions().Use<EpubModifier>().Build();
        }

        public static MarkdownPipeline Preview
        {
            get => new MarkdownPipelineBuilder().UseAdvancedExtensions().Use<PreviewModifier>().Build();
        }
        
        public static MarkdownPipeline Wordpress
        {
            get => new MarkdownPipelineBuilder().UseAdvancedExtensions().Use<WordpressModifier>().Build();
        }

        public MarkdownPipeline? MarkdownPipeline { get; private set; }

        public BookGenPipeline(MarkdownPipeline pipeline)
        {
            MarkdownPipeline = pipeline;
        }

        public void InjectRuntimeConfig(IReadonlyRuntimeSettings? runtimeConfig)
        {
            if (MarkdownPipeline == null) return;

            foreach (var extension in MarkdownPipeline.Extensions)
            {
                if (extension is IMarkdownExtensionWithRuntimeConfig configurable)
                {
                    configurable.RuntimeConfig = runtimeConfig;
                }
            }
        }

        public void InjectPath(FsPath path)
        {
            if (MarkdownPipeline == null) return;

            foreach (var extension in MarkdownPipeline.Extensions)
            {
                if (extension is IMarkdownExtensionWithPath hasPath)
                {
                    hasPath.Path = path;
                }
            }
        }

        public void SetSyntaxHighlightTo(bool enabled)
        {
            if (MarkdownPipeline == null) return;

            foreach (var extension in MarkdownPipeline.Extensions)
            {
                if (extension is IMarkdownExtensionWithSyntaxToggle toggle)
                {
                    toggle.SyntaxEnabled = enabled;
                }
            }
        }

        /// <summary>
        /// Generate Markdown to html
        /// </summary>
        /// <param name="markdown">Markdown to render</param>
        /// <returns>Html text</returns>
        public string RenderMarkdown(string markdown)
        {
            return Markdig.Markdown.ToHtml(markdown, MarkdownPipeline);
        }

        public void Dispose()
        {
            if (MarkdownPipeline == null) return;

            foreach (var extension in MarkdownPipeline.Extensions)
            {
                if (extension is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            MarkdownPipeline = null;
        }
    }
}
