using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Epub;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.idpf.org/2007/opf")]
public sealed class PackageSpineItemref
{
    [XmlAttribute(AttributeName = "idref")]
    public required string Idref { get; set; }

    [XmlAttribute(AttributeName = "linear")]
    public required string Linear { get; set; }
}
