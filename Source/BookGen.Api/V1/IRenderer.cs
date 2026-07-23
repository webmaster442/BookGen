//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.V1;

/// <summary>
/// Represents a renderer that can convert Markdown content to HTML.
/// </summary>
public interface IRenderer : IDisposable
{
    /// <summary>
    /// Renders the specified Markdown content to HTML.
    /// </summary>
    /// <param name="markdown">The Markdown content to render.</param>
    /// <returns>The rendered HTML content.</returns>
    string RenderMarkdownToRawHtml(string markdown);

    /// <summary>
    /// Renders the specified Markdown content to HTML using the provided page template and document.
    /// </summary>
    /// <param name="pageTemplate">The page template to use for rendering.</param>
    /// <param name="document">The document containing the Markdown content.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the rendered HTML content.</returns>
    Task<string> RenderMarkdownToHtml(string pageTemplate, IDocument document);

    /// <summary>
    /// Renders the specified Markdown content to HTML using the provided page template and document data.
    /// </summary>
    /// <param name="pageTemplate">The page template to use for rendering.</param>
    /// <param name="docData">The document data containing the Markdown content and front matter.</param>
    /// <returns>The rendered HTML content.</returns>
    string RenderMarkdownToHtml(string pageTemplate, (string content, IDocumentFrontMatter frontMatter) docData);

    /// <summary>
    /// Renders the specified Markdown content to HTML using the provided page template and tags.
    /// </summary>
    /// <param name="pageTemplate">The page template to use for rendering.</param>
    /// <param name="tags">The tags containing the content and metadata for rendering.</param>
    /// <returns>The rendered HTML content.</returns>
    string RenderMarkdownToHtml(string pageTemplate, RenderTags tags);
}
