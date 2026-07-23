//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Lib.Domain.Epub;

[Serializable()]
[XmlType(AnonymousType = true, Namespace = "urn:oasis:names:tc:opendocument:xmlns:container")]
public sealed class ContainerRootfiles
{
    [XmlElement("rootfile")]
    public required ContainerRootfilesRootfile Rootfile { get; set; }
}
