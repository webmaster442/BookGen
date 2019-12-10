//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Epub.Ncx
{
    [XmlRoot(ElementName = "ncx", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class Ncx
    {
        [XmlElement(ElementName = "head", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public Head? Head { get; set; }
        [XmlElement(ElementName = "docTitle", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public DocTitle? DocTitle { get; set; }
        [XmlElement(ElementName = "navMap", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public NavMap? NavMap { get; set; }
        [XmlElement(ElementName = "pageList", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public PageList? PageList { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string? Xmlns { get; set; }
        [XmlAttribute(AttributeName = "ncx", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string? _ncx { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string? Version { get; set; }
        [XmlAttribute(AttributeName = "lang", Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string? Lang { get; set; }
    }

}
