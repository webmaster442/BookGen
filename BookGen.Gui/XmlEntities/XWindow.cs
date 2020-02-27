using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookGen.Gui.XmlEntities
{
    public class XWindow
    {
        [XmlAttribute]
        public string Title { get; set; }
        
        [XmlArray]
        [XmlArrayItem(nameof(XLabel), typeof(XLabel))]
        [XmlArrayItem(nameof(XButton), typeof(XButton))]
        public List<XView> Children { get; set; }
    }
}
