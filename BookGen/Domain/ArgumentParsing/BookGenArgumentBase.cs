//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Ui.ArgumentParser;
using System;

namespace BookGen.Domain.ArgumentParsing
{
    internal class BookGenArgumentBase: ArgumentsBase
    {
        [Switch("v", "verbose")]
        public bool Verbose { get; set; }

        [Switch("d", "dir")]
        public string Directory { get; set; }

        public BookGenArgumentBase()
        {
            Directory = Environment.CurrentDirectory;
        }
    }
}
