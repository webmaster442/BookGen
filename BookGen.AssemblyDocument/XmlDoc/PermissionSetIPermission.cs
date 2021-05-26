using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace BookGen.AssemblyDocument
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    public class PermissionSetIPermission
    {
        public PermissionSetIPermission()
        {
            Unrestricted = false;
            Class = string.Empty;
            Version = string.Empty;
            Flags = string.Empty;
        }

        [XmlAnyElement]
        public XmlElement? Any { get; set; }

        [XmlAttribute("class")]
        public string Class { get; set; }

        [XmlAttribute("version", DataType = "integer")]
        public string Version { get; set; }

        [XmlAttribute]
        public string Flags { get; set; }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool Unrestricted { get; set; }

        [XmlAnyAttribute]
        public XmlAttribute[]? AnyAttr { get; set; }
    }
}