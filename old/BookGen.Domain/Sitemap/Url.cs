//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Sitemap
{
    [XmlRoot(ElementName = "url", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class Url
    {
        [XmlElement(ElementName = "loc", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
        public string? Loc { get; set; }

        [XmlElement(ElementName = "lastmod", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
        public string? Lastmod { get; set; }
    }
}
