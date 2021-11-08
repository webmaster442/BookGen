//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Gui.XmlEntities
{
    public record XLabel : XView
    {
        [XmlAttribute]
        public string Text { get; set; }

        public XLabel()
        {
            Text = string.Empty;
        }
    }
}
