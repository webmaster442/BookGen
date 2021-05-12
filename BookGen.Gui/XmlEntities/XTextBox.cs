//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Ui.XmlEntities
{
    public record XTextBox : XView
    {
        public string Text { get; set; }

        public XTextBox()
        {
            Text = string.Empty;
        }
    }
}
