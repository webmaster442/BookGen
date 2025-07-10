using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Epub;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.idpf.org/2007/opf")]
public sealed class PackageMetadata
{
    [XmlElement(ElementName = "identifier", Namespace = "http://purl.org/dc/elements/1.1/")]
    public required Identifier Identifier { get; set; }

    [XmlElement(ElementName = "title", Namespace = "http://purl.org/dc/elements/1.1/")]
    public required Title Title { get; set; }

    [XmlElement(ElementName = "date", Namespace = "http://purl.org/dc/elements/1.1/")]
    public required Date Date { get; set; }

    [XmlElement(ElementName = "language", Namespace = "http://purl.org/dc/elements/1.1/")]
    public required string Language { get; set; }

    [XmlElement("meta")]
    public required PackageMetadataMeta[] Meta { get; set; }
}
