//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Schema;
using System.Xml.Serialization;

namespace BookGen.Domain.Rss;

/// <summary>
/// Represents a Really Simple Syndication (RSS) feed
/// </summary>
[Serializable]
[XmlRoot(Namespace = "", IsNullable = false)]
public class RssFeed
{
    /// <summary>
    /// Creates a new instance of RSS
    /// </summary>
    public RssFeed()
    {
        Version = "2.0";
    }

    /// <summary>
    /// The RSS channel element describes the RSS feed.
    /// </summary>
    [XmlElement("channel", Form = XmlSchemaForm.Unqualified)]
    public required Channel Channel { get; set; }

    /// <summary>
    /// Feed version
    /// </summary>
    [XmlAttribute("version")]
    public string Version { get; }
}
