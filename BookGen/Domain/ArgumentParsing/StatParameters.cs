//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Ui.ArgumentParser;
using System;

namespace BookGen.Domain.ArgumentParsing
{
    internal class StatParameters : ArgumentsBase
    {
        [Switch("d", "dir")]
        public string Directory { get; set; }

        [Switch("i", "input")]
        public string Input { get; set; }

        public StatParameters()
        {
            Directory = Environment.CurrentDirectory;
            Input = string.Empty;
        }
    }
}
