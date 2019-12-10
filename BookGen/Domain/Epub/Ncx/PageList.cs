//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookGen.Domain.Epub.Ncx
{
    [XmlRoot(ElementName = "pageList", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class PageList
    {
        [XmlElement(ElementName = "navInfo", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public NavInfo? NavInfo { get; set; }
        [XmlElement(ElementName = "pageTarget", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public List<PageTarget>? PageTarget { get; set; }
    }
}
