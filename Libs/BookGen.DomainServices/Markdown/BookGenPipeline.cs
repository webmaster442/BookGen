//-----------------------------------------------------------------------------
// (c) 2020-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices.Markdown.Modifiers;
using BookGen.DomainServices.Markdown.TableOfContents;
using BookGen.Interfaces;
using Markdig;

namespace BookGen.DomainServices.Markdown
{
    public sealed class BookGenPipeline : IDisposable
    {
        public static MarkdownPipeline Web
        {
            get => new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseTableOfContents()
                .Use<WebModifier>()
                .Build();
        }

        public static MarkdownPipeline Print
        {
            get => new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseTableOfContents()
                .Use<PrintModifier>()
                .Build();
        }

        public static MarkdownPipeline Plain
        {
            get => new MarkdownPipelineBuilder()
                .Build();
        }

        public static MarkdownPipeline Epub
        {
            get => new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseTableOfContents()
                .Use<EpubModifier>()
                .Build();
        }

        public static MarkdownPipeline Preview
        {
            get => new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseTableOfContents()
                .Use<PreviewModifier>()
                .Build();
        }

        public static MarkdownPipeline Wordpress
        {
            get => new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseTableOfContents()
                .Use<WordpressModifier>()
                .Build();
        }

        public MarkdownPipeline? MarkdownPipeline { get; private set; }

        public BookGenPipeline(MarkdownPipeline pipeline)
        {
            MarkdownPipeline = pipeline;
        }

        public void InjectRuntimeConfig(IReadonlyRuntimeSettings? runtimeConfig)
        {
            if (MarkdownPipeline == null) return;

            foreach (IMarkdownExtension? extension in MarkdownPipeline.Extensions)
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

            foreach (IMarkdownExtension? extension in MarkdownPipeline.Extensions)
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

            foreach (IMarkdownExtension? extension in MarkdownPipeline.Extensions)
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

            foreach (IMarkdownExtension? extension in MarkdownPipeline.Extensions)
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
