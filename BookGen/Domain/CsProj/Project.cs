//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.CsProj
{
    [XmlRoot(ElementName = "Project")]
    public class Project
    {
        [XmlElement(ElementName = "PropertyGroup")]
        public PropertyGroup? PropertyGroup { get; set; }
        [XmlElement(ElementName = "ItemGroup")]
        public ItemGroup? ItemGroup { get; set; }
        [XmlAttribute(AttributeName = "Sdk")]
        public string? Sdk { get; set; }
    }
}
