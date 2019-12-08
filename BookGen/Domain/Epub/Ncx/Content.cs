//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Epub.Ncx
{
    [XmlRoot(ElementName = "content", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class Content
    {
        [XmlAttribute(AttributeName = "src")]
        public string Src { get; set; }
    }
}
