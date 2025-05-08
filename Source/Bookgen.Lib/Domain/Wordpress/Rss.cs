using System.Xml;
using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Wordpress;

[XmlRoot(ElementName = "rss")]
public sealed class Rss
{
    [XmlElement(ElementName = "channel")]
    public Channel? Channel { get; set; }
    [XmlAttribute(AttributeName = "version")]
    public string? Version { get; set; }
    [XmlAttribute(AttributeName = "excerpt", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string? Excerpt { get; set; }
    [XmlAttribute(AttributeName = "content", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string? Content { get; set; }
    [XmlAttribute(AttributeName = "wfw", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string? Wfw { get; set; }
    [XmlAttribute(AttributeName = "dc", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string? Dc { get; set; }
    [XmlAttribute(AttributeName = "wp", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string? Wp { get; set; }
}
