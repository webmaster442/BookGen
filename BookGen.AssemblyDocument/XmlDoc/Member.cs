using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument
{
    [Serializable]
    public class Member
    {
        [XmlElement("PermissionSet", typeof(PermissionSet))]
        [XmlElement("code", typeof(Code))]
        [XmlElement("completionlist", typeof(CompletionList))]
        [XmlElement("example", typeof(Example))]
        [XmlElement("exception", typeof(Exception))]
        [XmlElement("filterpriority", typeof(string), DataType = "integer")]
        [XmlElement("include", typeof(Include))]
        [XmlElement("list", typeof(List))]
        [XmlElement("param", typeof(Param))]
        [XmlElement("permission", typeof(Permission))]
        [XmlElement("remarks", typeof(Remarks))]
        [XmlElement("returns", typeof(Returns))]
        [XmlElement("seealso", typeof(Seealso))]
        [XmlElement("summary", typeof(Summary))]
        [XmlElement("typeparam", typeof(Typeparam))]
        [XmlElement("value", typeof(Value))]
        public object[] Items { get; set; }


        [XmlAttribute("name")]
        public string Name { get; set; }

        public Member()
        {
            Name = string.Empty;
            Items = Array.Empty<object>();
        }
    }
}