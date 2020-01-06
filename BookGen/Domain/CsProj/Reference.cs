//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.CsProj
{
    [XmlRoot(ElementName = "Reference")]
    public class Reference
    {
        [XmlElement(ElementName = "HintPath")]
        public string? HintPath { get; set; }
        [XmlAttribute(AttributeName = "Include")]
        public string? Include { get; set; }
    }
}
