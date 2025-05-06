using System.Xml;
using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Wordpress;

[XmlRoot(ElementName = "postmeta", Namespace = "http://wordpress.org/export/1.2/")]
public class Postmeta
{
    [XmlElement(ElementName = "meta_key", Namespace = "http://wordpress.org/export/1.2/")]
    public CData? Meta_key { get; set; }
    [XmlElement(ElementName = "meta_value", Namespace = "http://wordpress.org/export/1.2/")]
    public CData? Meta_value { get; set; }
}
