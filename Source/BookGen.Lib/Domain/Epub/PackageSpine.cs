//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Lib.Domain.Epub;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.idpf.org/2007/opf")]
public sealed class PackageSpine
{
    [XmlElement("itemref")]
    public required List<PackageSpineItemref> Itemref { get; set; }

    [XmlAttribute(AttributeName = "toc")]
    public required string Toc { get; set; }
}
