//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace Bang.Model;

[Serializable]
public class Site
{
    [XmlAttribute]
    public string Name { get; init; }

    [XmlAttribute]
    public string Url { get; init; }

    [XmlAttribute]
    public string Space { get; init; }

    public Site()
    {
        Name = string.Empty;
        Url = string.Empty;
        Space = string.Empty;
    }
}