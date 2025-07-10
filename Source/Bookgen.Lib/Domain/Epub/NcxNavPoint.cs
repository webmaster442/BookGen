using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Epub;

[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
public sealed class NcxNavPoint
{
    [XmlElement(ElementName = "navLabel")]
    public required NcxNavInfoType NavLabel { get; set; }

    [XmlElement(ElementName = "content")]
    public required NcxNavPointContent Content { get; set; }

    [XmlArray(ElementName = "navPoint")]
    public required NcxNavPoint[] NavPoint { get; set; }

    [XmlAttribute(AttributeName = "id")]
    public required string Id { get; set; }
}
