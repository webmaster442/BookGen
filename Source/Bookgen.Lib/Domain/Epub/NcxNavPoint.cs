//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
    public List<NcxNavPoint>? NavPoint { get; set; }

    [XmlAttribute(AttributeName = "id")]
    public required string Id { get; set; }
}
