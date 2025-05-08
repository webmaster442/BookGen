//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Epub
{
    [XmlRoot(ElementName = "meta", Namespace = "http://www.idpf.org/2007/opf")]
    public class Meta
    {
        [XmlAttribute(AttributeName = "property")]
        public string? Property { get; set; }

        [XmlText]
        public string? Text { get; set; }

        [XmlAttribute(AttributeName = "refines")]
        public string? Refines { get; set; }
    }
}
