//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Wordpress
{
    [XmlRoot(ElementName = "guid")]
    public class Guid
    {
        [XmlAttribute(AttributeName = "isPermaLink")]
        public bool IsPermaLink { get; set; }
        [XmlText]
        public string? Text { get; set; }
    }
}
