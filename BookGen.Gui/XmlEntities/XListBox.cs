//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Ui.XmlEntities
{
    public record XListBox : XView
    {
        [XmlAttribute]
        public string ItemSourceProperty { get; set; }

        [XmlAttribute]
        public int SelectedIndex { get; set; }

        public XListBox()
        {
            ItemSourceProperty = string.Empty;
            SelectedIndex = -1;
        }
    }
}
