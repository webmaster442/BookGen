//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.V1;

/// <summary>
/// Represents a renderer that can convert Markdown content to HTML.
/// </summary>
public interface IRenderer
{
    /// <summary>
    /// Renders the specified Markdown content to HTML.
    /// </summary>
    /// <param name="markdown">The Markdown content to render.</param>
    /// <returns>The rendered HTML content.</returns>
    string RenderMarkdownToHtml(string markdown);
}
