//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.Configuration
{
    /// <summary>
    /// Additional style classes that will be aplied to html elements
    /// during rendering
    /// </summary>
    public interface IReadOnylStyleClasses
    {
        /// <summary>
        /// H1
        /// </summary>
        string Heading1 { get; }
        /// <summary>
        /// H2
        /// </summary>
        string Heading2 { get; }
        /// <summary>
        /// H3
        /// </summary>
        string Heading3 { get; }
        /// <summary>
        /// Img
        /// </summary>
        string Image { get; }
        /// <summary>
        /// Table
        /// </summary>
        string Table { get; }
        /// <summary>
        /// Blockquote
        /// </summary>
        string Blockquote { get; }
        /// <summary>
        /// Figure
        /// </summary>
        string Figure { get; }
        /// <summary>
        /// FigureCaption
        /// </summary>
        string FigureCaption { get; }
        /// <summary>
        /// A
        /// </summary>
        string Link { get; }
        /// <summary>
        /// Ol
        /// </summary>
        string OrderedList { get; }
        /// <summary>
        /// Ul
        /// </summary>
        string UnorederedList { get; }
        /// <summary>
        /// Li
        /// </summary>
        string ListItem { get; }
    }
}
