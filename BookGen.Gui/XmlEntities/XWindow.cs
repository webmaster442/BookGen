//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookGen.Ui.XmlEntities
{
    public record XWindow
    {
        [XmlAttribute]
        public string Title { get; set; }
        
        [XmlArray]
        [XmlArrayItem(nameof(XLabel), typeof(XLabel))]
        [XmlArrayItem(nameof(XButton), typeof(XButton))]
        [XmlArrayItem(nameof(XSpacer), typeof(XSpacer))]
        [XmlArrayItem(nameof(XTextBlock), typeof(XTextBlock))]
        [XmlArrayItem(nameof(XCheckBox), typeof(XCheckBox))]
        public List<XView> Children { get; set; }

        public XWindow()
        {
            Title = string.Empty;
            Children = new List<XView>();
        }
    }
}
