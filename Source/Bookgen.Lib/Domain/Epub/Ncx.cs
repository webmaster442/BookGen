//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Epub;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
[XmlRoot(Namespace = "http://www.daisy.org/z3986/2005/ncx/", IsNullable = false, ElementName = "ncx")]
public sealed class Ncx
{
    [XmlArray(ElementName = "head")]
    [XmlArrayItem("meta", IsNullable = false)]
    public required List<NcxMeta> Head { get; set; }

    [XmlElement(ElementName = "docTitle")]
    public required NcxNavInfoType DocTitle { get; set; }

    [XmlArray(ElementName = "navMap")]
    [XmlArrayItem("navPoint", IsNullable = false)]
    public required List<NcxNavPoint> NavMap { get; set; }

    [XmlAttribute(AttributeName ="version")]
    public required string Version { get; set; }
}
