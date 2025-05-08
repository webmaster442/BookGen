using System.Xml;
using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Wordpress;

[XmlRoot(ElementName = "channel")]
public sealed class Channel
{
    [XmlElement(ElementName = "title")]
    public string? Title { get; set; }
    [XmlElement(ElementName = "link")]
    public string? Link { get; set; }
    [XmlElement(ElementName = "description")]
    public string? Description { get; set; }
    [XmlElement(ElementName = "pubDate")]
    public string? PubDate { get; set; }
    [XmlElement(ElementName = "language")]
    public string? Language { get; set; }
    [XmlElement(ElementName = "wxr_version", Namespace = "http://wordpress.org/export/1.2/")]
    public string? Wxr_version { get; set; }
    [XmlElement(ElementName = "base_site_url", Namespace = "http://wordpress.org/export/1.2/")]
    public string? Base_site_url { get; set; }
    [XmlElement(ElementName = "base_blog_url", Namespace = "http://wordpress.org/export/1.2/")]
    public string? Base_blog_url { get; set; }
    [XmlElement(ElementName = "author", Namespace = "http://wordpress.org/export/1.2/")]
    public Author? Author { get; set; }
    [XmlElement(ElementName = "generator")]
    public string? Generator { get; set; }
    [XmlElement(ElementName = "item")]
    public List<Item>? Item { get; set; }
}
