//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api.Configuration;

namespace BookGen.Core.Configuration
{
    public sealed class StyleClasses : IReadOnylStyleClasses
    {
        [Doc("css classes for: <H1>", true)]
        public string Heading1
        {
            get;
            set;
        }

        [Doc("css classes for: <H2>", true)]
        public string Heading2
        {
            get;
            set;
        }

        [Doc("css classes for: <H3>", true)]
        public string Heading3
        {
            get;
            set;
        }

        [Doc("css classes for: <IMG>", true)]
        public string Image
        {
            get;
            set;
        }

        [Doc("css classes for: <TABLE>", true)]
        public string Table
        {
            get;
            set;
        }

        [Doc("css classes for: <BLOCKQUOTE>", true)]
        public string Blockquote
        {
            get;
            set;
        }

        [Doc("css classes for: <FIGURE>", true)]
        public string Figure
        {
            get;
            set;
        }

        [Doc("css classes for: <FIGCAPTION>", true)]
        public string FigureCaption
        {
            get;
            set;
        }

        [Doc("css classes for: <A>", true)]
        public string Link
        {
            get;
            set;
        }

        [Doc("css classes for: <OL>", true)]
        public string OrderedList
        {
            get;
            set;
        }

        [Doc("css classes for: <UL>", true)]
        public string UnorederedList
        {
            get;
            set;
        }

        [Doc("css classes for: <LI>", true)]
        public string ListItem
        {
            get;
            set;
        }

        public StyleClasses()
        {
            Heading1 = string.Empty;
            Heading2 = string.Empty;
            Heading3 = string.Empty;
            Image = string.Empty;
            Table = string.Empty;
            Blockquote = string.Empty;
            Figure = string.Empty;
            FigureCaption = string.Empty;
            Link = string.Empty;
            OrderedList = string.Empty;
            UnorederedList = string.Empty;
            ListItem = string.Empty;
        }
    }
}
