using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Epub;

[Serializable()]
[XmlType(AnonymousType = true, Namespace = "urn:oasis:names:tc:opendocument:xmlns:container")]
[XmlRoot(Namespace = "urn:oasis:names:tc:opendocument:xmlns:container", IsNullable = false, ElementName = "container")]
public sealed class Container
{
    [XmlElement("rootfiles")]
    public required ContainerRootfiles Rootfiles { get; set; }

    [XmlAttribute(AttributeName = "version")]
    public required string Version { get; set; }
}
