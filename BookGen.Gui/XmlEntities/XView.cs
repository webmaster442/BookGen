//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Ui.XmlEntities
{
    public class XView
    {
        [XmlAttribute]
        public int Left { get; set; }
    }
}
