//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Epub
{
    [XmlRoot(ElementName = "identifier", Namespace = "http://purl.org/dc/elements/1.1/")]
    public class Identifier
    {
        [XmlAttribute(AttributeName = "id")]
        public string? Id { get; set; }
        [XmlText]
        public string? Text { get; set; }
    }
}
