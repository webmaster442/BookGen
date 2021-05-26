using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument
{
    [Serializable]
    public class Remarks : Content
    {
        [XmlElement("para")]
        public Para[] Items1 { get; set; }

        public Remarks()
        {
            Items1 = Array.Empty<Para>();
        }
    }
}