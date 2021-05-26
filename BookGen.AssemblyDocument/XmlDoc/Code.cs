using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument.XmlDoc
{
    [Serializable]
    public class Code
    {
        [XmlAttribute("language")]
        public string Language { get; set; }


        [XmlText]
        public string[] Text { get; set; }

        public Code()
        {
            Language = string.Empty;
            Text = Array.Empty<string>();
        }
    }
}