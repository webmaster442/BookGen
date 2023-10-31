//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Www;

[Serializable]
[XmlType(AnonymousType = true)]
public class WwwBang
{
    [XmlAttribute("url")]
    public string Url { get; set; }

    [XmlAttribute("delimiter")]
    public string Delimiter { get; set; }

    [XmlAttribute("activator")]
    public string Activator { get; set; }

    [XmlText]
    public string Value { get; set; }

    public WwwBang()
    {
        Url = string.Empty;
        Delimiter = string.Empty;
        Activator = string.Empty;
        Value = string.Empty;
    }
}