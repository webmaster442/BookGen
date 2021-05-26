using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument
{
    [Serializable]
    public class Paramref
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public string[] Text { get; set; }

        public Paramref()
        {
            Name = string.Empty;
            Text = Array.Empty<string>();
        }
    }
}