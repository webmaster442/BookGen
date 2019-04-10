//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Epub
{
    [XmlRoot(ElementName = "container", Namespace = "urn:oasis:names:tc:opendocument:xmlns:container")]
    public class Container
    {
        [XmlElement(ElementName = "rootfiles", Namespace = "urn:oasis:names:tc:opendocument:xmlns:container")]
        public Rootfiles Rootfiles { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
}
