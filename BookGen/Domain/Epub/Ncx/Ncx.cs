using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookGen.Domain.Epub.Ncx
{
    [XmlRoot(ElementName = "meta", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class Meta
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "content")]
        public string Content { get; set; }
    }

    [XmlRoot(ElementName = "head", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class Head
    {
        [XmlElement(ElementName = "meta", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public List<Meta> Meta { get; set; }
    }

    [XmlRoot(ElementName = "docTitle", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class DocTitle
    {
        [XmlElement(ElementName = "text", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "navInfo", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class NavInfo
    {
        [XmlElement(ElementName = "text", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "navLabel", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class NavLabel
    {
        [XmlElement(ElementName = "text", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "content", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class Content
    {
        [XmlAttribute(AttributeName = "src")]
        public string Src { get; set; }
    }

    [XmlRoot(ElementName = "navPoint", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class NavPoint
    {
        [XmlElement(ElementName = "navLabel", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public NavLabel NavLabel { get; set; }
        [XmlElement(ElementName = "content", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public Content Content { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "navPoint", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public List<NavPoint> NavPoints { get; set; }
    }

    [XmlRoot(ElementName = "navMap", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class NavMap
    {
        [XmlElement(ElementName = "navInfo", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public NavInfo NavInfo { get; set; }
        [XmlElement(ElementName = "navPoint", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public NavPoint NavPoint { get; set; }
    }

    [XmlRoot(ElementName = "pageTarget", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class PageTarget
    {
        [XmlElement(ElementName = "navLabel", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public NavLabel NavLabel { get; set; }
        [XmlElement(ElementName = "content", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public Content Content { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }

    [XmlRoot(ElementName = "pageList", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class PageList
    {
        [XmlElement(ElementName = "navInfo", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public NavInfo NavInfo { get; set; }
        [XmlElement(ElementName = "pageTarget", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public List<PageTarget> PageTarget { get; set; }
    }

    [XmlRoot(ElementName = "ncx", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class Ncx
    {
        [XmlElement(ElementName = "head", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public Head Head { get; set; }
        [XmlElement(ElementName = "docTitle", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public DocTitle DocTitle { get; set; }
        [XmlElement(ElementName = "navMap", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public NavMap NavMap { get; set; }
        [XmlElement(ElementName = "pageList", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public PageList PageList { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "ncx", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string _ncx { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
        [XmlAttribute(AttributeName = "lang", Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string Lang { get; set; }
    }

}
