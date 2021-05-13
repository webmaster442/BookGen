//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookGen.Ui.XmlEntities
{
    public abstract record XChildContainer : XView
    {
        [XmlArray]
        [XmlArrayItem(nameof(XLabel), typeof(XLabel))]
        [XmlArrayItem(nameof(XButton), typeof(XButton))]
        [XmlArrayItem(nameof(XSpacer), typeof(XSpacer))]
        [XmlArrayItem(nameof(XTextBlock), typeof(XTextBlock))]
        [XmlArrayItem(nameof(XCheckBox), typeof(XCheckBox))]
        [XmlArrayItem(nameof(XListBox), typeof(XListBox))]
        [XmlArrayItem(nameof(XTextBox), typeof(XTextBox))]
        public List<XView> Children { get; set; }

        public XChildContainer()
        {
            Children = new List<XView>();
        }
    }
}
