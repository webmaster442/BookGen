//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookGen.Gui.XmlEntities
{
    public record XSPlitView : XView
    {
        [XmlArray]
        [XmlArrayItem(nameof(XListBox), typeof(XListBox))]
        [XmlArrayItem(nameof(XTextBox), typeof(XTextBox))]
        public List<XView> Children { get; set; }

        public XSPlitView()
        {
            Children = new List<XView>();
        }
    }
}
