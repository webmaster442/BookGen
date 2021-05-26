using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    public class ListItem
    {
        [XmlElement("term")]
        public string Term { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlText]
        public string[] Text { get; set; }

        public ListItem()
        {
            Term = string.Empty;
            Description = string.Empty;
            Text = Array.Empty<string>();
        }
    }
}