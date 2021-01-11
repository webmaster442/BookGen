//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Ui.XmlEntities
{
    public record XSpacer: XView
    {
        [XmlAttribute]
        public int Rows { get; set; }

        public XSpacer()
        {
            Rows = 1;
        }
    }
}
