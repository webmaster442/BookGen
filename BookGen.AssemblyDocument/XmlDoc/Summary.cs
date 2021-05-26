using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument.XmlDoc
{
    [Serializable]
    public class Summary : Content
    {
        [XmlElement("para")]
        public Para[] Items1 { get; set; }

        public Summary()
        {
            Items1 = Array.Empty<Para>();
        }
    }
}