//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.CsProj
{
    [XmlRoot(ElementName = "ItemGroup")]
    public class ItemGroup
    {
        [XmlElement(ElementName = "Reference")]
        public Reference? Reference { get; set; }
    }
}
