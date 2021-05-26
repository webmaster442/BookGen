using System;
using System.Xml.Serialization;

namespace BookGen.AssemblyDocument.XmlDoc
{
    [Serializable]
    public abstract class CrefText
    {
        [XmlAttribute("cref")]
        public string Cref { get; set; }


        [XmlText]
        public string[] Text { get; set; }

        public CrefText()
        {
            Cref = string.Empty;
            Text = Array.Empty<string>();
        }
    }
}
