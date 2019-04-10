//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

namespace BookGen.Core.Configuration
{
    public class StyleClasses
    {
        [Description("Additional style classes for <H1>")]
        public string Heading1 { get; set; }
        [Description("Additional style classes for <H2>")]
        public string Heading2 { get; set; }
        [Description("Additional style classes for <H3>")]
        public string Heading3 { get; set; }
        [Description("Additional style classes for <img>")]
        public string Image { get; set; }
        [Description("Additional style classes for <table>")]
        public string Table { get; set; }
        [Description("Additional style classes for <blocquoute>")]
        public string Blockquote { get; set; }
        [Description("Additional style classes for <Figure>")]
        public string Figure { get; set; }
        [Description("Additional style classes for <FigureCaption>")]
        public string FigureCaption { get; set; }
        [Description("Additional style classes for <a>")]
        public string Link { get;  set; }
        [Description("Additional style classes for <ul>")]
        public string OrderedList { get; set; }
        [Description("Additional style classes for <ol>")]
        public string UnorederedList { get; set; }
        [Description("Additional style classes for <li>")]
        public string ListItem { get; set; }

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
