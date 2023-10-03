//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Launcher.ViewModels.Rss;

[Serializable]
[XmlType(AnonymousType = true)]
public class RssChannelItem
{
    [XmlElement("title")]
    public string Title { get; set; }

    [XmlElement("link")]
    public string Link { get; set; }

    [XmlElement("description")]
    public string Description { get; set; }

    [XmlElement("pubDate")]
    public string PubDate { get; set; }

    [XmlElement("guid")]
    public RssChannelItemGuid Guid { get; set; }

    public RssChannelItem()
    {
        Title = string.Empty;
        Link = string.Empty;
        Description = string.Empty;
        PubDate = string.Empty;
        Guid = new RssChannelItemGuid();
    }
}
