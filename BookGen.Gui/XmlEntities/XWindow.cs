//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
        [XmlArrayItem(nameof(XSpacer), typeof(XSpacer))]
        [XmlArrayItem(nameof(XTextBlock), typeof(XTextBlock))]
        public List<XView> Children { get; set; }
    }
}
