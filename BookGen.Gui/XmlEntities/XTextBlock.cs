//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Gui.XmlEntities
{
    public class XTextBlock: XView
    {
        public CData Text { get; set; }

        public XTextBlock()
        {
            Text = new CData(string.Empty);
        }

    }
}
