using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Epub;

[XmlType(AnonymousType = true, Namespace = "http://purl.org/dc/elements/1.1/")]
[XmlRoot(Namespace = "http://purl.org/dc/elements/1.1/", IsNullable = false)]
public sealed class Creator
{
    [XmlText]
    public required string Value { get; set; }
}