using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument.XmlDoc
{
    [Serializable]
    public class Typeparamref
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public string[] Text { get; set; }

        public Typeparamref()
        {
            Name = string.Empty;
            Text = Array.Empty<string>();
        }
    }
}