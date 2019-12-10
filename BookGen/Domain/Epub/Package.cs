//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Epub
{
    [XmlRoot(ElementName = "package", Namespace = "http://www.idpf.org/2007/opf")]
    public class Package
    {
        [XmlElement(ElementName = "metadata", Namespace = "http://www.idpf.org/2007/opf")]
        public Metadata? Metadata { get; set; }
        [XmlElement(ElementName = "manifest", Namespace = "http://www.idpf.org/2007/opf")]
        public Manifest? Manifest { get; set; }
        [XmlElement(ElementName = "spine", Namespace = "http://www.idpf.org/2007/opf")]
        public Spine? Spine { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string? Xmlns { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string? Version { get; set; }
        [XmlAttribute(AttributeName = "lang", Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string? Lang { get; set; }
        [XmlAttribute(AttributeName = "unique-identifier")]
        public string? Uniqueidentifier { get; set; }
    }
}
