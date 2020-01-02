//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Core.Contracts.Configuration
{
    public interface IReadOnylStyleClasses
    {
        string Heading1 { get; }
        string Heading2 { get; }
        string Heading3 { get; }
        string Image { get; }
        string Table { get; }
        string Blockquote { get; }
        string Figure { get; }
        string FigureCaption { get; }
        string Link { get; }
        string OrderedList { get; }
        string UnorederedList { get; }
        string ListItem { get; }
    }
}
