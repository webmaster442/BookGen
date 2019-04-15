//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookGen.Domain.Epub
{
    [XmlRoot(ElementName = "metadata", Namespace = "http://www.idpf.org/2007/opf")]
    public class Metadata
    {
        [XmlElement(ElementName = "dc:title", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Title { get; set; }
        [XmlElement(ElementName = "dc:creator", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Creator { get; set; }
        [XmlElement(ElementName = "dc:language", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Language { get; set; }
        [XmlElement(ElementName = "dc:rights", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Rights { get; set; }
        [XmlElement(ElementName = "dc:publisher", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Publisher { get; set; }
        [XmlElement(ElementName = "dc:identifier", Namespace = "http://purl.org/dc/elements/1.1/")]
        public Identifier Identifier { get; set; }
        [XmlElement(ElementName = "meta", Namespace = "http://www.idpf.org/2007/opf")]
        public List<MetaOpf> Meta { get; set; }
        [XmlAttribute(AttributeName = "opf", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Opf { get; set; }
    }
}
