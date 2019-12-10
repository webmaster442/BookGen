//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookGen.Domain.Epub.Ncx
{
    [XmlRoot(ElementName = "head", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class Head
    {
        [XmlElement(ElementName = "meta", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public List<Meta>? Meta { get; set; }
    }
}
