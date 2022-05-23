//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Epub
{
    [XmlRoot(ElementName = "metadata", Namespace = "http://www.idpf.org/2007/opf")]
    public class Metadata
    {
        [XmlElement(ElementName = "identifier", Namespace = "http://purl.org/dc/elements/1.1/")]
        public Identifier? Identifier { get; set; }
        [XmlElement(ElementName = "meta")]
        public List<Meta>? Meta { get; set; }
        [XmlElement(ElementName = "title", Namespace = "http://purl.org/dc/elements/1.1/")]
        public List<Title>? Title { get; set; }
        [XmlElement(ElementName = "creator", Namespace = "http://purl.org/dc/elements/1.1/")]
        public List<Creator>? Creator { get; set; }
        [XmlElement(ElementName = "language", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string? Language { get; set; }
        [XmlElement(ElementName = "date", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string? Date { get; set; }
        [XmlElement(ElementName = "subject", Namespace = "http://purl.org/dc/elements/1.1/")]
        public List<string>? Subject { get; set; }
        [XmlElement(ElementName = "source", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string? Source { get; set; }
        [XmlElement(ElementName = "rights", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string? Rights { get; set; }
        [XmlAttribute(AttributeName = "dc", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string? Dc { get; set; }
    }
}
