//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookGen.Domain.Sitemap
{
    [XmlRoot(ElementName = "urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class UrlSet
    {
        [XmlElement(ElementName = "url", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
        public List<Url> Url { get; set; }

        public UrlSet()
        {
            Url = new List<Url>();
        }
    }

}
