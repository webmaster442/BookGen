using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Epub;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.idpf.org/2007/opf")]
public sealed class PackageItem
{
    [XmlAttribute(AttributeName = "id")]
    public required string Id { get; set; }

    [XmlAttribute(AttributeName = "href")]
    public required string Href { get; set; }

    [XmlAttribute("media-type")]
    public required string Mediatype { get; set; }

    [XmlAttribute(AttributeName = "properties")]
    public required string Properties { get; set; }
}
