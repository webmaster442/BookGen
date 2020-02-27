//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Gui.XmlEntities
{
    public class XButton : XView
    {
        [XmlAttribute]
        public string Action { get; set; }

        [XmlAttribute]
        public string Text { get; set; }
    }
}
