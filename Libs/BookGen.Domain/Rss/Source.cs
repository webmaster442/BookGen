//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Rss;

/// <summary>
/// Specifies a third-party source for the item
/// </summary>
[Serializable]
public class Source
{
    /// <summary>
    /// Specifies the link to the source
    /// </summary>
    [XmlAttribute("url")]
    public required string Url { get; set; }

    /// <summary>
    /// Value
    /// </summary>
    [XmlText]
    public required string Value { get; set; }
}
