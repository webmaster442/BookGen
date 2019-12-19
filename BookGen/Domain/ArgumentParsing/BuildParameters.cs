//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen.Domain.ArgumentParsing
{
    internal class BuildParameters
    {
        public ActionType? Action { get; set; }
        public bool NoWaitForExit { get; set; }
        public bool Verbose { get; set; }
        public string WorkDir { get; set; }

        public BuildParameters()
        {
            WorkDir = Environment.CurrentDirectory;
        }
    }
}
