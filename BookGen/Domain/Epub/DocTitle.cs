//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Epub
{
    [XmlRoot(ElementName = "docTitle", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class DocTitle
    {
        [XmlElement(ElementName = "text", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
        public string Text { get; set; }
    }

}
