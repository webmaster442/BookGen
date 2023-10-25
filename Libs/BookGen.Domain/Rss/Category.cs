//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Rss;

/// <summary>
/// The category element specifies one or more categories the channel or item belongs to. The category element can also specify the hierarchic location in a category (the value of the category element is then a forward-slash separated string).
/// </summary>
[Serializable]
public class Category
{
    /// <summary>
    /// string or URL that identifies a categorization taxonomy.
    /// </summary>
    [XmlAttribute("domain")]
    public string? Domain { get; set; }

    /// <summary>
    /// Value string
    /// </summary>
    [XmlText]
    public required string Value { get; set; }
}
