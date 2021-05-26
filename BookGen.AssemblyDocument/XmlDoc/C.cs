using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument
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