using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument.XmlDoc
{
    [Serializable]
    public class Include
    {
        [XmlAttribute("file")]
        public string File { get; set; }

        [XmlAttribute("path")]
        public string Path { get; set; }

        public Include()
        {
            File = string.Empty;
            Path = string.Empty;
        }
    }
}