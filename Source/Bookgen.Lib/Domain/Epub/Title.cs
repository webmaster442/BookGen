using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Epub;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://purl.org/dc/elements/1.1/")]
[XmlRoot(Namespace = "http://purl.org/dc/elements/1.1/", IsNullable = false)]
public sealed class Title
{
    [XmlAttribute(AttributeName = "id")]
    public required string Id { get; set; }

    [XmlText]
    public required string Value { get; set; }
}
