using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument
{
    [Serializable]
    public class List
    {
        [XmlElement("listheader")]
        public ListListheader Listheader { get; set; }

        [XmlElement("item")]
        public ListItem[] Item { get; set; }

        [XmlAttribute]
        public string Type { get; set; }

        public List()
        {
            Listheader = new ListListheader();
            Item = Array.Empty<ListItem>();
            Type = string.Empty;
        }
    }
}