using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument
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