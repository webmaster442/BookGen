//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Epub;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.idpf.org/2007/opf")]
public sealed class PackageGuideReference
{
    [XmlAttribute(AttributeName = "type")]
    public required string Type { get; set; }

    [XmlAttribute(AttributeName = "title")]
    public required string Title { get; set; }

    [XmlAttribute(AttributeName = "href")]
    public required string Href { get; set; }
}
