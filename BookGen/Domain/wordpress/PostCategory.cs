//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Wordpress
{
    [Serializable]
    public class PostCategory
    {
        [XmlAttribute("domain")]
        public string? Domain { get; set; }

        [XmlAttribute("nicename")]
        public string? Nicename { get; set; }

        [XmlText]
        public string? Value { get; set; }
    }
}
