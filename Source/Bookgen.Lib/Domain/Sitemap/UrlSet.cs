using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Sitemap;

[XmlRoot(ElementName = "urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
public sealed class UrlSet
{
    [XmlElement(ElementName = "url", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public List<Url> Url { get; set; }

    public UrlSet()
    {
        Url = new List<Url>();
    }
}