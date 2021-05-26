using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument.XmlDoc
{
    [Serializable]
    public class Param : Content
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        public Param()
        {
            Name = string.Empty;
        }
    }
}