using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument.XmlDoc
{
    [Serializable]
    public class PermissionSet
    {
        [XmlElement("IPermission")]
        public PermissionSetIPermission[] IPermission { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        public PermissionSet()
        {
            Type = string.Empty;
            IPermission = Array.Empty<PermissionSetIPermission>();
        }
    }
}