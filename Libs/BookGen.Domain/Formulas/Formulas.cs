using System.Xml.Serialization;

namespace BookGen.Domain.Formulas;

[XmlRoot("formulas")]
public sealed class Formulas
{
    [XmlArray("f")]
    [XmlArrayItem("formula")]
    public CData[] Items { get; set; }

    public Formulas()
    {
        Items = [];
    }
}
