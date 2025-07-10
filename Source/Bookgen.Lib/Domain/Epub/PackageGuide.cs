using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Epub;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.idpf.org/2007/opf")]
public sealed class PackageGuide
{
    [XmlElement(ElementName = "reference")]
    public required PackageGuideReference Reference { get; set; }
}
