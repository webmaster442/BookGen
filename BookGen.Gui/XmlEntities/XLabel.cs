using System.Xml.Serialization;

namespace BookGen.Gui.XmlEntities
{
    public class XLabel: XView
    {
        [XmlAttribute]
        public string Text { get; set; }
    }
}
