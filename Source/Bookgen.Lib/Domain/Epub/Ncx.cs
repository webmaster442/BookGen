using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Epub;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
[XmlRoot(Namespace = "http://www.daisy.org/z3986/2005/ncx/", IsNullable = false)]
public sealed class Ncx
{
    [XmlElement(ElementName = "head")]
    [XmlArrayItem("meta", IsNullable = false)]
    public required NcxMeta[] Head { get; set; }

    [XmlElement(ElementName = "docTitle")]
    public required NcxNavInfoType DocTitle { get; set; }

    [XmlElement(ElementName = "navMap")]
    [XmlArrayItem("navPoint", IsNullable = false)]
    public required NcxNavPoint[] NavMap { get; set; }

    [XmlAttribute(AttributeName ="version")]
    public required string Version { get; set; }
}
