using System.Xml.Serialization;

namespace BookGen.Domain.Epub
{
    [XmlRoot(ElementName = "ncx", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class Ncx
    {
        [XmlElement(ElementName = "head", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public Head Head { get; set; }

        [XmlElement(ElementName = "docTitle", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public DocTitle DocTitle { get; set; }

        [XmlElement(ElementName = "docAuthor", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public DocAuthor DocAuthor { get; set; }

        [XmlElement(ElementName = "navMap", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public NavMap NavMap { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
    }
}
