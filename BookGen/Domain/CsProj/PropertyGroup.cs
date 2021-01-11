//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.CsProj
{
    [XmlRoot(ElementName = "PropertyGroup")]
    public record PropertyGroup
    {
        [XmlElement(ElementName = "TargetFramework")]
        public string? TargetFramework { get; set; }
        [XmlElement(ElementName = "Nullable")]
        public string? Nullable { get; set; }
    }
}
