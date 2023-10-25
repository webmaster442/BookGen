//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Rss;

/// <summary>
/// The image element allows an image to be displayed when aggregators present a feed.
/// </summary>
[Serializable]
public class Image
{
    private int? _width;
    private int? _height;

    /// <summary>
    /// Specifies the URL to the image
    /// </summary>
    [XmlElement("url", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public required string Url { get; set; }

    /// <summary>
    /// Defines the text to display if the image could not be shown
    /// </summary>
    [XmlElement("title", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public required string Title { get; set; }

    /// <summary>
    /// Defines the hyperlink to the website that offers the channel
    /// </summary>
    [XmlElement("link", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public required string Link { get; set; }

    /// <summary>
    /// Defines the width of the image. Default is 88. Maximum value is 144
    /// </summary>
    [XmlElement("width", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public int? Width
    {
        get => _width ?? 88;
        set
        {
            if (value.HasValue && value > 144)
                _width = 144;
            else
                _width = value;
        }
    }

    /// <summary>
    /// Defines the height of the image. Default is 31. Maximum value is 400
    /// </summary>
    [XmlElement("height", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public int? Height
    {
        get => _height ?? 31;
        set
        {
            if (value.HasValue && value > 400)
                _height = 400;
            else
                _height = value;
        }
    }

    /// <summary>
    /// Specifies the text in the HTML title attribute of the link around the image
    /// </summary>
    [XmlElement("description", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string? Description { get; set; }
}
