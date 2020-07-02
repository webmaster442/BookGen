//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Ui.XmlEntities
{
    public class XCheckBox: XView
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
