using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Bookgen.Lib.Domain.Wordpress;

public class CData : IXmlSerializable
{
    /// <summary>
    /// Allow direct assignment from string:
    /// CData cdata = "abc";
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static implicit operator CData(string value)
        => new CData(value);

    /// <summary>
    /// Allow direct assigment to string
    /// string str = cdata;
    /// </summary>
    /// <param name="cdata"></param>
    /// <returns></returns>
    public static implicit operator string(CData cdata) 
        => cdata.Value;

    [XmlIgnore]
    public string Value { get; set; }

    public CData() : this(string.Empty)
    {
    }

    public CData(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;

    public XmlSchema? GetSchema() => null;

    public virtual void ReadXml(XmlReader reader)
    {
        Value = reader.ReadElementString();
    }

    public virtual void WriteXml(XmlWriter writer)
        => writer.WriteCData(Value);
}