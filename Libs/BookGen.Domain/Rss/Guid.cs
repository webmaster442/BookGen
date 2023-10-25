//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Rss;

/// <summary>
/// The guid element defines a unique identifier for the item.
/// </summary>
[Serializable]
public class Guid
{
    /// <summary>
    /// Optional. If set to true, the reader may assume that it is a permalink to the item (a url that points to the full item described by the item element). The default value is true. If set to false, the guid may not be assumed to be a url
    /// </summary>
    [XmlAttribute("isPermaLink")]
    public bool IsPermaLink { get; set; }

    /// <summary>
    /// Value
    /// </summary>
    [XmlText]
    public required string Value { get; set; }
}
