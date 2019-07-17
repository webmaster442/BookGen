//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookGen.Domain.Epub
{
    [XmlRoot(ElementName = "spine", Namespace = "http://www.idpf.org/2007/opf")]
    public class Spine
    {
        [XmlElement(ElementName = "itemref", Namespace = "http://www.idpf.org/2007/opf")]
        public List<Itemref> Itemref { get; set; }

        [XmlAttribute(AttributeName = "toc")]
        public string Toc { get; set; }
    }
}
