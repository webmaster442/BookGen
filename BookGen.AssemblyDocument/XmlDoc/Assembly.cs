using System;

namespace BookGen.AssemblyDocument.XmlDoc
{
    [Serializable]
    public class Assembly
    {
        public string name { get; set; }

        public Assembly()
        {
            name = string.Empty;
        }
    }
}