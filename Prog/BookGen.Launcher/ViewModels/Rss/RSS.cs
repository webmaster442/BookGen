//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Launcher.ViewModels.Rss;

[Serializable]
[XmlType(AnonymousType = true)]
[XmlRoot(Namespace = "", IsNullable = false)]
public class RSS
{
    [XmlElement("channel")]
    public RssChannel Channel { get; set; }

    [XmlAttribute("version")]
    public string Version { get; set; }

    public RSS()
    {
        Channel = new RssChannel();
        Version = string.Empty;
    }
}
