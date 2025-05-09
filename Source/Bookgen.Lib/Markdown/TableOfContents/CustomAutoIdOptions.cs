//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
// Based on work of Alexandre Mutel. https://github.com/leisn/MarkdigToc
//-----------------------------------------------------------------------------

using Markdig.Extensions.AutoIdentifiers;

namespace Bookgen.Lib.Markdown.TableOfContents;

/// <summary>
/// Delegate for handle custom heading id generate.
/// </summary>
/// <param name="level">The level of current heading, usually be count of char <strong>'#'</strong></param>
/// <param name="content">The content of current heading.</param>
/// <param name="id">Not null if already defined id in markdown strings (e.g. <em># title {#<strong>id</strong>}</em>)</param>
/// <returns>Generated Heading id</returns>
internal delegate string GenerateHeadingId(int level, string content, string? id);

/// <summary>
/// Options for CustomAutoIdExtension
/// </summary>
internal sealed class CustomAutoIdOptions
{
    /// <summary>
    /// Options for generate heading id
    /// </summary>
    public AutoIdentifierOptions Options { get; set; } = AutoIdentifierOptions.Default;

    /// <summary>
    /// Heading id generator.<br/>
    /// Notice: If already defined id in markdown strings (e.g. <em># title {#<strong>id</strong>}</em>), will pass by the <strong>id</strong> parameter .<br/>
    /// When set this with a <strong>not null</strong> value, the <see cref="Options"/> will be <strong>Ignored</strong>.<br/>
    /// </summary>
    public GenerateHeadingId? HeadingIdGenerator { get; set; }
}
