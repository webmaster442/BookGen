using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument.XmlDoc
{
    [Serializable]
    public class Exception : Content
    {
        [XmlAttribute("cref")]
        public string Cref { get; set; }

        public Exception()
        {
            Cref = string.Empty;
        }
    }
}