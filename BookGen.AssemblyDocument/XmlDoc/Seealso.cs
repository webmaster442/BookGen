using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument
{
    [Serializable]
    public class Seealso
    {
        [XmlAttribute("cref")]
        public string Cref { get; set; }

        [XmlText]
        public string[] Text { get; set; }

        public Seealso()
        {
            Cref = string.Empty;
            Text = Array.Empty<string>();
        }
    }
}