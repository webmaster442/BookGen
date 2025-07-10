using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Epub;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.idpf.org/2007/opf")]
public sealed class PackageMetadataMeta
{
    [XmlAttribute(AttributeName = "property")]
    public required string Property { get; set; }

    [XmlText]
    public required string Value { get; set; }
}
