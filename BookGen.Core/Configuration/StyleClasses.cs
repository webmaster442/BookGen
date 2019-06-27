//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Core.Configuration
{
    public class StyleClasses
    {
        public string Heading1
        {
            get;
            set;
        }

        public string Heading2
        {
            get;
            set;
        }

        public string Heading3
        {
            get;
            set;
        }

        public string Image
        {
            get;
            set;
        }

        public string Table
        {
            get;
            set;
        }

        public string Blockquote
        {
            get;
            set;
        }

        public string Figure
        {
            get;
            set;
        }

        public string FigureCaption
        {
            get;
            set;
        }

        public string Link
        {
            get;
            set;
        }

        public string OrderedList
        {
            get;
            set;
        }

        public string UnorederedList
        {
            get;
            set;
        }

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
