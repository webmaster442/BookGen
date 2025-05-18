using System.Xml;
using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Wordpress;

[XmlRoot(ElementName = "author", Namespace = "http://wordpress.org/export/1.2/")]
public sealed class Author
{
    [XmlElement(ElementName = "author_id", Namespace = "http://wordpress.org/export/1.2/")]
    public required string AuthorId { get; init; }

    [XmlElement(ElementName = "author_login", Namespace = "http://wordpress.org/export/1.2/")]
    public required CData AuthorLogin { get; init; }

    [XmlElement(ElementName = "author_email", Namespace = "http://wordpress.org/export/1.2/")]
    public required CData AuthorEmail { get; init; }

    [XmlElement(ElementName = "author_display_name", Namespace = "http://wordpress.org/export/1.2/")]
    public required CData AuthorDisplayName { get; init; }

    [XmlElement(ElementName = "author_first_name", Namespace = "http://wordpress.org/export/1.2/")]
    public required CData AuthorFirstName { get; init; }

    [XmlElement(ElementName = "author_last_name", Namespace = "http://wordpress.org/export/1.2/")]
    public required CData AuthorLastName { get; init; }
}
