//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Rss;

/// <summary>
/// The enclosure child element allows a media-file to be included with an item.
/// </summary>
[Serializable]
public class Enclosure
{
    /// <summary>
    /// Defines the URL to the media file
    /// </summary>
    [XmlAttribute("url")]
    public required string Url { get; set; }

    /// <summary>
    /// Defines the length (in bytes) of the media file
    /// </summary>
    [XmlAttribute("length")]
    public required int Length { get; set; }

    /// <summary>
    /// Defines the Mime type of media file.
    /// </summary>
    [XmlAttribute("type")]
    public required string Type { get; set; }
}
