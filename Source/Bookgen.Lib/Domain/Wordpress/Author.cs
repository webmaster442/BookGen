using System.Xml;
using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Wordpress;

[XmlRoot(ElementName = "author", Namespace = "http://wordpress.org/export/1.2/")]
public sealed class Author
{
    [XmlElement(ElementName = "author_id", Namespace = "http://wordpress.org/export/1.2/")]
    public string? Author_id { get; set; }
    [XmlElement(ElementName = "author_login", Namespace = "http://wordpress.org/export/1.2/")]
    public CData? Author_login { get; set; }
    [XmlElement(ElementName = "author_email", Namespace = "http://wordpress.org/export/1.2/")]
    public CData? Author_email { get; set; }
    [XmlElement(ElementName = "author_display_name", Namespace = "http://wordpress.org/export/1.2/")]
    public CData? Author_display_name { get; set; }
    [XmlElement(ElementName = "author_first_name", Namespace = "http://wordpress.org/export/1.2/")]
    public CData? Author_first_name { get; set; }
    [XmlElement(ElementName = "author_last_name", Namespace = "http://wordpress.org/export/1.2/")]
    public CData? Author_last_name { get; set; }
}
