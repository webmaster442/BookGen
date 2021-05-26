using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument
{
    [Serializable]
    public class CompletionList
    {
        public CompletionList()
        {
            Cref = string.Empty;
            Text = Array.Empty<string>();
        }

        [XmlAttribute("cref")]
        public string Cref { get; set; }


        [XmlText]
        public string[] Text { get; set; }
    }
}