using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Epub;

[Serializable()]
[XmlType(AnonymousType = true, Namespace = "urn:oasis:names:tc:opendocument:xmlns:container")]
public sealed class ContainerRootfilesRootfile
{
    [XmlAttribute("full-path")]
    public required string Fullpath { get; set; }

    [XmlAttribute("media-type")]
    public required string Mediatype { get; set; }
}