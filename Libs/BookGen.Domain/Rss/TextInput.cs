//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Rss;

/// <summary>
/// Specifies a text input field that should be displayed with the feed
/// </summary>
[Serializable]
public class TextInput
{

    /// <summary>
    /// Defines the label of the submit button in the text input area
    /// </summary>
    [XmlElement("title", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public required string Title { get; set; }

    /// <summary>
    /// Defines a description of the text input area
    /// </summary>
    [XmlElement("description", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public required string Description { get; set; }

    /// <summary>
    /// Defines the name of the text object in the text input area
    /// </summary>
    [XmlElement("name", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public required string Name { get; set; }

    /// <summary>
    /// Defines the URL of the CGI script that processes the text input
    /// </summary>
    [XmlElement("link", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public required string Link { get; set; }
}
