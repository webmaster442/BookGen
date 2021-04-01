//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Ui.ArgumentParser;
using System;

namespace BookGen.Domain.ArgumentParsing
{
    internal class BuildParameters: BookGenArgumentBase
    {
        [Switch("a", "action", true)]
        public BuildAction? Action { get; set; }

        [Switch("n", "nowait")]
        public bool NoWaitForExit { get; set; }
    }
}
