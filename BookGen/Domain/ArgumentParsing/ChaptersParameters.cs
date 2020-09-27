//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Ui.ArgumentParser;
using System;

namespace BookGen.Domain.ArgumentParsing
{
    internal class ChaptersParameters : ArgumentsBase
    {
        [Switch("a", "action", true)]
        public ChaptersAction? Action { get; set; }

        [Switch("d", "dir")]
        public string WorkDir { get; set; }

        public ChaptersParameters()
        {
            WorkDir = Environment.CurrentDirectory;
        }
    }
}
