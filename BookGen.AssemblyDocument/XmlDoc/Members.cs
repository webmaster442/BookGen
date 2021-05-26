using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument
{
    [Serializable]
    public class Members
    {
        [XmlElement("member")]
        public Member[] Items { get; set; }

        public Members()
        {
            Items = Array.Empty<Member>();
        }
    }
}