using System.Xml;
using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Wordpress;

[XmlRoot(ElementName = "channel")]
public sealed class Channel
{
    [XmlElement(ElementName = "title")]
    public required string Title { get; init; }

    [XmlElement(ElementName = "link")]
    public required string Link { get; init; }

    [XmlElement(ElementName = "description")]
    public required string Description { get; init; }

    [XmlElement(ElementName = "pubDate")]
    public required string PubDate { get; init; }

    [XmlElement(ElementName = "language")]
    public required string Language { get; init; }

    [XmlElement(ElementName = "wxr_version", Namespace = "http://wordpress.org/export/1.2/")]
    public required string WxrVersion { get; init; }

    [XmlElement(ElementName = "base_site_url", Namespace = "http://wordpress.org/export/1.2/")]
    public required string BaseSiteUrl { get; init; }

    [XmlElement(ElementName = "base_blog_url", Namespace = "http://wordpress.org/export/1.2/")]
    public required string BaseBlogUrl { get; init; }

    [XmlElement(ElementName = "author", Namespace = "http://wordpress.org/export/1.2/")]
    public required Author Author { get; init; }

    [XmlElement(ElementName = "generator")]
    public required string Generator { get; init; }

    [XmlElement(ElementName = "item")]
    public required List<Item> Item { get; init; }
}
