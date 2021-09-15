//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.ArgumentParser;
using System;

namespace BookGen.Domain.ArgumentParsing
{
    internal class StatArguments : ArgumentsBase
    {
        [Switch("d", "dir")]
        public string Directory { get; set; }

        [Switch("i", "input")]
        public string Input { get; set; }

        public StatArguments()
        {
            Directory = Environment.CurrentDirectory;
            Input = string.Empty;
        }
    }
}
