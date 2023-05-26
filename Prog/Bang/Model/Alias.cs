//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace Bang.Model;

[Serializable]
public class Alias
{
    [XmlAttribute]
    public string Name { get; init; }

    [XmlAttribute]
    public string Sites { get; init; }

    [XmlIgnore]
    public IEnumerable<string> SiteNames => Sites.Split(';');

    public Alias()
    {
        Name = string.Empty;
        Sites = string.Empty;
    }
}
