//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen.Domain.ArgumentParsing
{
    internal class PageGenParameters
    {
        public PageType? PageType { get; set; }
        public string WorkDir { get; set; }

        public PageGenParameters()
        {
            WorkDir = Environment.CurrentDirectory;
        }
    }
}
