using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Epub;

[Serializable]
[XmlRoot(Namespace = "http://www.idpf.org/2007/opf", IsNullable = false, ElementName = "package")]
public sealed class Package
{
    [XmlElement("metadata")]
    public required PackageMetadata Metadata { get; set; }

    [XmlArray("manifest")]
    [XmlArrayItem("item", IsNullable = false)]
    public required List<PackageItem> Manifest { get; set; }

    [XmlElement("spine")]
    public required PackageSpine Spine { get; set; }

    [XmlElement("guide")]
    public PackageGuide? Guide { get; set; }

    [XmlAttribute(AttributeName = "version")]
    public required string Version { get; set; }

    [XmlAttribute(AttributeName = "lang", Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
    public required string Lang { get; set; }

    [XmlAttribute("unique-identifier")]
    public required string Uniqueidentifier { get; set; }

    [XmlAttribute(AttributeName = "prefix")]
    public required string Prefix { get; set; }
}
