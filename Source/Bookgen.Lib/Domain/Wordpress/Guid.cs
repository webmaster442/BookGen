//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml;
using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Wordpress;

[XmlRoot(ElementName = "guid")]
public sealed class Guid
{
    [XmlAttribute(AttributeName = "isPermaLink")]
    public bool IsPermaLink { get; set; }
    [XmlText]
    public string? Text { get; set; }
}
