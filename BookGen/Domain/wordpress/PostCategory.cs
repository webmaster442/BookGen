//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml;
using System.Xml.Serialization;

namespace BookGen.Domain.Wordpress
{
    [XmlRoot(ElementName = "category")]
    public class PostCategory : CData
    {
        public string? Domain { get; set; }

        public string? Nicename { get; set; }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("domain", Domain);
            writer.WriteAttributeString("nicename", Nicename);
            base.WriteXml(writer);
        }
    }
}
