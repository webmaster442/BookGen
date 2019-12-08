//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Epub.Ncx
{
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
}
