//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Epub
{
    [XmlRoot(ElementName = "rootfile")]
    public class Rootfile
    {
        [XmlAttribute(AttributeName = "full-path")]
        public string Fullpath { get; set; }

        [XmlAttribute(AttributeName = "media-type")]
        public string Mediatype { get; set; }
    }
}
