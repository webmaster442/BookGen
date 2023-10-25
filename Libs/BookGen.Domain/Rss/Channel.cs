//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Schema;
using System.Xml.Serialization;

namespace BookGen.Domain.Rss;

/// <summary>
/// This element is used to describe the RSS feed.
/// </summary>
[Serializable]
public class Channel
{
    /// <summary>
    /// The name of the channel.It's how people refer
    /// to your service.If you have an HTML website
    /// that contains the same information as your RSS
    /// file, the title of your channel should be the
    /// same as the title of your website.
    /// </summary>
    [XmlElement("title", Form = XmlSchemaForm.Unqualified)]
    public required string Title { get; init; }

    /// <summary>
    /// The URL to the HTML website corresponding to the channel.
    /// </summary>
    [XmlElement("link", Form = XmlSchemaForm.Unqualified)]
    public required string Link { get; init; }

    /// <summary>
    /// Phrase or sentence describing the channel.
    /// </summary>
    [XmlElement("description", Form = XmlSchemaForm.Unqualified)]
    public required string Description { get; init; }

    /// <summary>
    /// The language the channel is written in. This
    /// allows aggregators to group all Italian language
    /// sites, for example, on a single page.A list of
    /// allowable values for this element, as provided
    /// by Netscape, is here http://www.rssboard.org/rss-language-codes
    /// </summary>
    /// <remarks>
    /// You may also use values defined by the W3C http://www.w3.org/TR/REC-html40/struct/dirlang.html#langcodes
    /// </remarks>
    [XmlElement("language", Form = XmlSchemaForm.Unqualified)]
    public string? Language { get; init; }

    /// <summary>
    /// Copyright notice for content in the channel.
    /// </summary>
    [XmlElement("copyright", Form = XmlSchemaForm.Unqualified)]
    public string? Copyright { get; init; }

    /// <summary>
    /// Email address for person responsible for editorial content.
    /// </summary>
    [XmlElement("managingEditor", Form = XmlSchemaForm.Unqualified)]
    public string? ManagingEditor { get; init; }

    /// <summary>
    /// Email address for person responsible for technical issues relating to channel.
    /// </summary>
    [XmlElement("webMaster", Form = XmlSchemaForm.Unqualified)]
    public string? WebMaster { get; init; }

    /// <summary>
    /// The publication date for the content in the
    /// channel. For example, the New York Times
    /// publishes on a daily basis, the publication date
    /// flips once every 24 hours. That's when the
    /// pubDate of the channel changes. All date-times
    /// in RSS conform to the Date and Time
    /// Specification of RFC 822, with the exception
    /// that the year may be expressed with two
    /// characters or four characters (four preferred).
    /// </summary>
    [XmlElement("pubDate", Form = XmlSchemaForm.Unqualified)]
    public string? PubDate { get; init; }

    /// <summary>
    /// The last time the content of the channel changed.
    /// </summary>
    [XmlElement("lastBuildDate", Form = XmlSchemaForm.Unqualified)]
    public string? LastBuildDate { get; init; }

    /// <summary>
    /// Specify one or more categories that the channel
    /// belongs to. Follows the same rules as the
    /// item-level category element
    /// </summary>
    [XmlElement("category", Form = XmlSchemaForm.Unqualified)]
    public Category[]? Category { get; init; }

    /// <summary>
    /// A string indicating the program used to generate the channel.
    /// </summary>
    [XmlElement("generator", Form = XmlSchemaForm.Unqualified)]
    public string? Generator { get; init; }

    /// <summary>
    /// Allows processes to register with a cloud to be
    /// notified of updates to the channel, implementing
    /// a lightweight publish-subscribe protocol for RSS
    /// feeds
    /// </summary>
    [XmlElement("cloud", Form = XmlSchemaForm.Unqualified)]
    public Cloud? Cloud { get; init; }

    /// <summary>
    /// ttl stands for time to live. It's a number of
    /// minutes that indicates how long a channel can be
    /// cached before refreshing from the source.
    /// </summary>
    [XmlElement("ttl", Form = XmlSchemaForm.Unqualified)]
    public int? Ttl { get; init; }

    /// <summary>
    /// Specifies a GIF, JPEG or PNG image that can be displayed with the channel.
    /// </summary>
    [XmlElement("image", Form = XmlSchemaForm.Unqualified)]
    public Image? Image { get; init; }

    /// <summary>
    /// The PICS [http://www.w3.org/PICS/] rating for the channel.
    /// </summary>
    [XmlElement("rating", Form = XmlSchemaForm.Unqualified)]
    public string? Rating { get; init; }

    /// <summary>
    /// Specifies a text input box that can be displayed with the channel.
    /// </summary>
    [XmlElement("textInput", Form = XmlSchemaForm.Unqualified)]
    public TextInput? TextInput { get; init; }

    /// <summary>
    /// A hint for aggregators telling them which hours
    /// they can skip. This element contains up to 24
    /// hour sub-elements whose value is a
    /// number between 0 and 23, representing a time in
    /// GMT, when aggregators, if they support the
    /// feature, may not read the channel on hours
    /// listed in the skipHours element. The
    /// hour beginning at midnight is hour zero.
    /// </summary>
    [XmlArray("skipHours", Form = XmlSchemaForm.Unqualified)]
    [XmlArrayItem("hour", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
    public int[]? SkipHours { get; init; }

    /// <summary>
    /// A hint for aggregators telling them which days
    /// they can skip. This element contains up to seven
    /// day sub-elements whose value is Monday, Tuesday,
    /// Wednesday, Thursday, Friday, Saturday or Sunday. Aggregators may not read the channel
    /// during days listed in the skipDays element
    /// </summary>
    [XmlArray("skipDays", Form = XmlSchemaForm.Unqualified)]
    [XmlArrayItem("day", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
    public SkipDaysDay[]? SkipDays { get; init; }

    /// <summary>
    /// Each item element defines an article or "story" in an RSS feed.
    /// </summary>
    [XmlElement("item", Form = XmlSchemaForm.Unqualified)]
    public Item[]? Item { get; init; }
}