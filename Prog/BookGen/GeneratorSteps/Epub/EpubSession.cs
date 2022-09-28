//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.GeneratorSteps.Epub
{
    internal sealed class EpubSession
    {
        public List<string> GeneratedFiles { get; }

        public EpubSession()
        {
            GeneratedFiles = new List<string>();
        }
    }
}
