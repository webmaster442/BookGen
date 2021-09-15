//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.ArgumentParser;
using System;

namespace BookGen.Domain.ArgumentParsing
{
    internal class ChaptersArguments : ArgumentsBase
    {
        [Switch("a", "action", true)]
        public ChaptersAction? Action { get; set; }

        [Switch("d", "dir")]
        public string WorkDir { get; set; }

        public ChaptersArguments()
        {
            WorkDir = Environment.CurrentDirectory;
        }
    }
}
