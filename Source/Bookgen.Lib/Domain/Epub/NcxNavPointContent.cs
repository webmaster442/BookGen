using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Epub;

[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
public sealed class NcxNavPointContent
{
    [XmlAttribute(AttributeName = "src")]
    public required string Src { get; set; }
}