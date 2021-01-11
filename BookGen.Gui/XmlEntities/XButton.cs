//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Ui.XmlEntities
{
    public record XButton : XView
    {
        [XmlAttribute]
        public string Command { get; set; }

        [XmlAttribute]
        public string Text { get; set; }

        public XButton()
        {
            Text = string.Empty;
            Command = string.Empty;
        }
    }
}
