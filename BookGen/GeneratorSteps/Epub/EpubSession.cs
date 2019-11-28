//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.GeneratorSteps.Epub
{
    internal class EpubSession
    {
        public List<string> GeneratedFiles { get; }

        public EpubSession()
        {
            GeneratedFiles = new List<string>();
        }
    }
}
