//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Ui.XmlEntities
{
    public record XTextBox : XView
    {
        [XmlAttribute]
        public string Text { get; set; }

        [XmlAttribute]
        public bool IsReadonly { get; set; }

        public XTextBox()
        {
            Text = string.Empty;
        }
    }
}
