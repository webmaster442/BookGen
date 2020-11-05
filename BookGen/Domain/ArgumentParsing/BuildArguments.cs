//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Ui.ArgumentParser;
using System;

namespace BookGen.Domain.ArgumentParsing
{
    internal class BuildArguments: ArgumentsBase
    {
        [Switch("a", "action", true)]
        public BuildAction? Action { get; set; }

        [Switch("n", "nowait")]
        public bool NoWaitForExit { get; set; }

        [Switch("v", "verbose")]
        public bool Verbose { get; set; }

        [Switch("d", "dir")]
        public string WorkDir { get; set; }

        public BuildArguments()
        {
            WorkDir = Environment.CurrentDirectory;
        }
    }
}
