//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Gui.XmlEntities
{
    public sealed record XRadioGroup : XView
    {
        [XmlArray]
        [XmlArrayItem(ElementName = "Option")]
        public string[] Options { get; set; }

        [XmlAttribute]
        public string SelectedIndex { get; set; }

        public XRadioGroup()
        {
            Options = Array.Empty<string>();
            SelectedIndex = "-1";
        }
    }
}
