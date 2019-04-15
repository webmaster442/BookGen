//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookGen.Domain.Epub
{
    [XmlRoot(ElementName = "navMap", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class NavMap
    {
        [XmlElement(ElementName = "navPoint", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public List<NavPoint> NavPoint { get; set; }
    }
}
