using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument
{
    [Serializable]
    public class Typeparam : Content
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        public Typeparam()
        {
            Name = string.Empty;
        }
    }
}