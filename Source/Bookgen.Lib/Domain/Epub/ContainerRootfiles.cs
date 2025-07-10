using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Epub;

[Serializable()]
[XmlType(AnonymousType = true, Namespace = "urn:oasis:names:tc:opendocument:xmlns:container")]
public sealed class ContainerRootfiles
{
    [XmlElement("rootfile")]
    public required ContainerRootfilesRootfile Rootfile { get; set; }
}
