//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BookGen.Domain
{
    public class CData : IXmlSerializable
    {

        /// <summary>
        /// Allow direct assignment from string:
        /// CData cdata = "abc";
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator CData(string value)
        {
            return new CData(value);
        }

        /// <summary>
        /// Allow direct assigment to string
        /// string str = cdata;
        /// </summary>
        /// <param name="cdata"></param>
        /// <returns></returns>
        public static implicit operator string(CData cdata)
        {
            return cdata.Value;
        }

        [XmlIgnore]
        public string Value { get; set; }

        public CData() : this(string.Empty)
        {
        }

        public CData(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public XmlSchema? GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(XmlReader reader)
        {
            Value = reader.ReadElementString();
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteCData(Value);
        }
    }
}
