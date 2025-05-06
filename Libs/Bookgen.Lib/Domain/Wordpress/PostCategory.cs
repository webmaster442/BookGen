using System.Xml;
using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Wordpress;

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
