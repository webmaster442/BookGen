//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Epub
{
    [XmlRoot(ElementName = "itemref", Namespace = "http://www.idpf.org/2007/opf")]
    public class Itemref
    {
        [XmlAttribute(AttributeName = "idref")]
        public string Idref { get; set; }
    }
}
