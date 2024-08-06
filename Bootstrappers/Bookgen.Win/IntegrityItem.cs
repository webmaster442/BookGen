using System;
using System.Xml.Serialization;

namespace Bookgen.Win
{
    [XmlRoot(ElementName = "i")]
    [Serializable]
    public sealed class IntegrityItem
    {
        [XmlAttribute(AttributeName = "f")]
        public string FileName { get; set; }

        [XmlAttribute(AttributeName = "h")]
        public string Hash { get; set; }
    }
}
