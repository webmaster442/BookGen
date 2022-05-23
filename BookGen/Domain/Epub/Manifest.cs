//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Epub
{
    [XmlRoot(ElementName = "manifest", Namespace = "http://www.idpf.org/2007/opf")]
    public class Manifest
    {
        [XmlElement(ElementName = "item", Namespace = "http://www.idpf.org/2007/opf")]
        public List<Item>? Item { get; set; }
    }
}
