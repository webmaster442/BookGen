//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Gui.XmlEntities
{
    public class XCheckBox: XView
    {
        public string IsChecked { get; set; }
        public string Text { get; set; }

        public XCheckBox()
        {
            IsChecked = string.Empty;
            Text = string.Empty;
        }
    }
}
