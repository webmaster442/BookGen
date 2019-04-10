//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Epub
{
    [XmlRoot(ElementName = "rootfiles", Namespace = "urn:oasis:names:tc:opendocument:xmlns:container")]
    public class Rootfiles
    {
        [XmlElement(ElementName = "rootfile", Namespace = "urn:oasis:names:tc:opendocument:xmlns:container")]
        public Rootfile Rootfile { get; set; }
    }
}
