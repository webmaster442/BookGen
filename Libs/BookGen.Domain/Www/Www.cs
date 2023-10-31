//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Www;

[Serializable]
[XmlType(TypeName = "www")]
public class Www
{
    [XmlArrayItem("url", IsNullable = false)]
    [XmlElement("favorites")]
    public WwwUrl[] Favorites { get; set; }

    [XmlArrayItem("bang", IsNullable = false)]
    [XmlElement("bangs")]
    public WwwBang[] Bangs { get; set; }

    public Www()
    {
        Favorites = Array.Empty<WwwUrl>();
        Bangs = Array.Empty<WwwBang>();
    }
}
