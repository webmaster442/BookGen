//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Www;

[Serializable()]
[XmlType(AnonymousType = true)]
public class WwwUrl
{
    [XmlAttribute("href")]
    public string Href { get; set; }

    [XmlText()]
    public string Value { get; set; }

    public WwwUrl()
    {
        Href = string.Empty;
        Value = string.Empty;
    }
}
