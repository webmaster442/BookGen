//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Rss;

/// <summary>
/// Each item element defines an article or "story" in an RSS feed.
/// </summary>
[Serializable]
public class Item
{
    /// <summary>
    /// The title of the item.
    /// </summary>
    [XmlElement("title", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public required string Title { get; set; }

    /// <summary>
    /// The URL of the item.
    /// </summary>
    [XmlElement("link", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public required string Link { get; set; }

    /// <summary>
    /// The item synopsis.
    /// </summary>
    [XmlElement("description", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public required string Description { get; set; }

    /// <summary>
    /// Email address of the author of the item.
    /// </summary>
    [XmlElement("author", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string? Author { get; set; }

    /// <summary>
    /// Includes the item in one or more categories.
    /// </summary>
    [XmlElement("category", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public Category[]? Category { get; set; }

    /// <summary>
    /// URL of a page for comments relating to the item.
    /// </summary>
    [XmlElement("comments", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string? Comments { get; set; }

    /// <summary>
    /// Describes a media object that is attached to the item.
    /// </summary>
    [XmlElement("enclosure", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public Enclosure? Enclosure { get; set; }

    /// <summary>
    /// A string that uniquely identifies the item.
    /// </summary>
    [XmlElement("guid", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public Guid? Guid { get; set; }

    /// <summary>
    /// Defines the last-publication date for the item
    /// </summary>
    [XmlElement("pubDate", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string? PubDate { get; set; }

    /// <summary>
    /// The RSS channel that the item came from.
    /// </summary>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public Source? Source { get; set; }
}
