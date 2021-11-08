//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.ArgumentParser;

namespace BookGen.Domain.ArgumentParsing
{
    internal class MdTableArguments : ArgumentsBase
    {
        [Switch("d", "delimiter")]
        public char Delimiter { get; set; }

        public MdTableArguments()
        {
            Delimiter = '\t';
        }
    }
}
