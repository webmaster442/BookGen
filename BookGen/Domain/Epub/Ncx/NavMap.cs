//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Epub.Ncx
{
    [XmlRoot(ElementName = "navMap", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class NavMap
    {
        [XmlElement(ElementName = "navInfo", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public NavInfo NavInfo { get; set; }
        [XmlElement(ElementName = "navPoint", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public NavPoint NavPoint { get; set; }
    }
}
