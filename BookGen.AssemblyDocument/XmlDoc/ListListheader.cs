using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    public class ListListheader
    {
        [XmlElement("term")]
        public string Term { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        public ListListheader()
        {
            Term = string.Empty;
            Description = string.Empty;
        }
    }
}