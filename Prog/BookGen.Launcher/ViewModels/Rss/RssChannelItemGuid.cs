//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Launcher.ViewModels.Rss;

[Serializable]
[XmlType(AnonymousType = true)]
public class RssChannelItemGuid
{
    [XmlAttribute("isPermaLink")]
    public bool IsPermaLink { get; set; }

    [XmlText()]
    public string Value { get; set; }

    public RssChannelItemGuid()
    {
        Value = string.Empty;
    }
}