using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument.XmlDoc
{
    [Serializable]
    [XmlRoot("doc")]
    public class Doc
    {
        [XmlElement("assembly")]
        public Assembly Assembly { get; set; }

        [XmlElement("members")]
        public Members Members { get; set; }

        public Doc()
        {
            Assembly = new Assembly();
            Members = new Members();
        }
    }
}