//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookGen.Domain.wordpress
{
    [XmlRoot(ElementName = "item")]
    public class Item
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        [XmlElement(ElementName = "link")]
        public string Link { get; set; }
        [XmlElement(ElementName = "pubDate")]
        public string PubDate { get; set; }
        [XmlElement(ElementName = "creator", Namespace = "http://purl.org/dc/elements/1.1/")]
        public CData Creator { get; set; }
        [XmlElement(ElementName = "guid")]
        public Guid Guid { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "encoded", Namespace = "http://purl.org/rss/1.0/modules/content/")]
        public CData Content { get; set; }
        [XmlElement(ElementName = "post_id", Namespace = "http://wordpress.org/export/1.2/")]
        public int Post_id { get; set; }
        [XmlElement(ElementName = "post_date", Namespace = "http://wordpress.org/export/1.2/")]
        public CData Post_date { get; set; }
        [XmlElement(ElementName = "post_date_gmt", Namespace = "http://wordpress.org/export/1.2/")]
        public CData Post_date_gmt { get; set; }
        [XmlElement(ElementName = "comment_status", Namespace = "http://wordpress.org/export/1.2/")]
        public CData Comment_status { get; set; }
        [XmlElement(ElementName = "ping_status", Namespace = "http://wordpress.org/export/1.2/")]
        public CData Ping_status { get; set; }
        [XmlElement(ElementName = "post_name", Namespace = "http://wordpress.org/export/1.2/")]
        public CData Post_name { get; set; }
        [XmlElement(ElementName = "status", Namespace = "http://wordpress.org/export/1.2/")]
        public string Status { get; set; }
        [XmlElement(ElementName = "post_parent", Namespace = "http://wordpress.org/export/1.2/")]
        public int Post_parent { get; set; }
        [XmlElement(ElementName = "menu_order", Namespace = "http://wordpress.org/export/1.2/")]
        public int Menu_order { get; set; }
        [XmlElement(ElementName = "post_type", Namespace = "http://wordpress.org/export/1.2/")]
        public CData Post_type { get; set; }
        [XmlElement(ElementName = "post_password", Namespace = "http://wordpress.org/export/1.2/")]
        public CData Post_password { get; set; }
        [XmlElement(ElementName = "is_sticky", Namespace = "http://wordpress.org/export/1.2/")]
        public string Is_sticky { get; set; }
        [XmlElement(ElementName = "postmeta", Namespace = "http://wordpress.org/export/1.2/")]
        public List<Postmeta> Postmeta { get; set; }
    }
}
