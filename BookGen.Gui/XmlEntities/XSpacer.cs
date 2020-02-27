//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Gui.XmlEntities
{
    public class XSpacer: XView
    {
        [XmlAttribute]
        public int Rows { get; set; }

        public XSpacer()
        {
            Rows = 1;
        }
    }
}
