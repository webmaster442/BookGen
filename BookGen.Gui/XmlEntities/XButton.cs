using System.Xml.Serialization;

namespace BookGen.Gui.XmlEntities
{
    public class XButton : XView
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Text { get; set; }
    }
}
