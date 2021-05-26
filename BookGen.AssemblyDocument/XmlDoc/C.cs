using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument.XmlDoc
{
    [Serializable]
    public class C
    {
        [XmlText]
        public string[] Text { get; set; }

        public C()
        {
            Text = Array.Empty<string>();
        }
    }
}