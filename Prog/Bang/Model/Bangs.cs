//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace Bang.Model;

[XmlRoot]
[Serializable]
internal class Bangs
{
    [XmlAttribute("Version")]
    public string VersionString { get; init; }

    [XmlArray]
    [XmlArrayItem("a", typeof(Alias))]
    public Alias[] Aliases { get; init; }


    [XmlArray]
    [XmlArrayItem("s", typeof(Site))]
    public Site[] Sites { get; init; }

    public Bangs()
    {
        VersionString = "1.0";
        Aliases = Array.Empty<Alias>();
        Sites = Array.Empty<Site>();
    }
}
