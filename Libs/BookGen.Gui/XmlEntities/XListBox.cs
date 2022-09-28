//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Gui.XmlEntities
{
    public sealed record XListBox : XView
    {
        [XmlAttribute]
        public string Title { get; set; }

        [XmlAttribute]
        public string ItemSourceProperty { get; set; }

        [XmlAttribute]
        public string SelectedIndex { get; set; }

        public XListBox()
        {
            Title = string.Empty;
            ItemSourceProperty = string.Empty;
            SelectedIndex = "-1";
        }
    }
}
