//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Gui.XmlEntities
{
    public record XCheckBox: XView
    {
        [XmlAttribute]
        public string IsChecked { get; set; }

        [XmlAttribute]
        public string Text { get; set; }

        public XCheckBox()
        {
            IsChecked = string.Empty;
            Text = string.Empty;
        }
    }
}
