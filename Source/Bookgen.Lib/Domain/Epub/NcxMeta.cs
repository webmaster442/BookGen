using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Epub;

[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
public sealed class NcxMeta
{
    [XmlAttribute(AttributeName ="name")]
    public required string Name { get; set; }

    [XmlAttribute(AttributeName = "content")]
    public required string Content { get; set; }
}
