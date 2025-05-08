using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Sitemap;

[XmlRoot(ElementName = "url", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
public sealed class Url
{
    [XmlElement(ElementName = "loc", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public string? Loc { get; set; }

    [XmlElement(ElementName = "lastmod", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public string? Lastmod { get; set; }
}
