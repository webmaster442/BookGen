//-----------------------------------------------------------------------------
// (c) 2020-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Interfaces.Configuration
{
    /// <summary>
    /// Additional style classes that will be aplied to html elements
    /// during rendering
    /// </summary>
    public interface IReadOnylStyleClasses
    {
        /// <summary>
        /// Style classed for H1 elements
        /// </summary>
        string Heading1 { get; }
        /// <summary>
        /// Style classed for H2 elements
        /// </summary>
        string Heading2 { get; }
        /// <summary>
        /// Style classed for H3 elements
        /// </summary>
        string Heading3 { get; }
        /// <summary>
        /// Img
        /// </summary>
        string Image { get; }
        /// <summary>
        /// Style classed for Table elements
        /// </summary>
        string Table { get; }
        /// <summary>
        /// Style classed for Blockquote elements
        /// </summary>
        string Blockquote { get; }
        /// <summary>
        /// Style classed for Figure elements
        /// </summary>
        string Figure { get; }
        /// <summary>
        /// Style classed for FigureCaption elements
        /// </summary>
        string FigureCaption { get; }
        /// <summary>
        /// Style classed for A elements
        /// </summary>
        string Link { get; }
        /// <summary>
        /// Style classed for Ol elements
        /// </summary>
        string OrderedList { get; }
        /// <summary>
        /// Style classed for Ul elements
        /// </summary>
        string UnorederedList { get; }
        /// <summary>
        /// Style classed for Li elements
        /// </summary>
        string ListItem { get; }
    }
}
