﻿//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Ui.XmlEntities
{
    public record XTextBlock: XView
    {
        public CData Text { get; set; }

        public XTextBlock()
        {
            Text = new CData(string.Empty);
        }

    }
}
