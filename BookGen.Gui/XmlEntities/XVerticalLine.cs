//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Ui.XmlEntities
{
    public record XVerticalLine : XView
    {
        [XmlAttribute]
        public int Height { get; set; }

        [XmlAttribute]
        public char Symbol { get; set; }

        public XVerticalLine()
        {
            Symbol = '|';
        }
    }
}
