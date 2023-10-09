//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Launcher.ViewModels.Rss;

[Serializable]
public class RssChannel
{
    [XmlElement("title")]
    public string Title { get; set; }

    [XmlElement("link")]
    public string Link { get; set; }

    [XmlElement("description")]
    public string Description { get; set; }

    [XmlElement("language")]
    public string Language { get; set; }

    [XmlElement("lastBuildDate")]
    public string LastBuildDate { get; set; }

    [XmlElement("item")]
    public RssChannelItem[] Item { get; set; }

    public RssChannel()
    {
        Title = string.Empty;
        Link = string.Empty;
        Description = string.Empty;
        Language = string.Empty;
        LastBuildDate = string.Empty;
        Item = Array.Empty<RssChannelItem>();
    }
}
