//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.V1;

/// <summary>
/// Represents CSS classes that can be applied to various HTML elements during rendering.
/// </summary>
public sealed class CssClasses
{
    /// <summary>
    /// css classes aplied to `h1` element
    /// </summary>
    public string H1 { get; set; } = string.Empty;

    /// <summary>
    /// css classes aplied to `h2` element
    /// </summary>
    public string H2 { get; set; } = string.Empty;

    /// <summary>
    /// css classes aplied to `h3` element
    /// </summary>
    public string H3 { get; set; } = string.Empty;

    /// <summary>
    /// css classes aplied to `img` element
    /// </summary>
    public string Img { get; set; } = string.Empty;

    /// <summary>
    /// css classes aplied to `table` element
    /// </summary>
    public string Table { get; set; } = string.Empty;

    /// <summary>
    /// css classes aplied to `blockquote` element
    /// </summary>
    public string Blockquote { get; set; } = string.Empty;
    
    /// <summary>   
    /// css classes aplied to `figure` element
    /// </summary>
    public string Figure { get; set; } = string.Empty;

    /// <summary>
    /// css classes aplied to `figcaption` element
    /// </summary>
    public string FigureCaption { get; set; } = string.Empty;

    /// <summary>
    /// css classes aplied to `a` element
    /// </summary>
    public string Link { get; set; } = string.Empty;

    /// <summary>
    /// css classes aplied to `ol` element
    /// </summary>
    public string Ol { get; set; } = string.Empty;

    /// <summary>
    /// css classes aplied to `ul` element
    /// </summary>
    public string Ul { get; set; } = string.Empty;

    /// <summary>
    /// css classes aplied to `li` element
    /// </summary>
    public string Li { get; set; } = string.Empty;
}
