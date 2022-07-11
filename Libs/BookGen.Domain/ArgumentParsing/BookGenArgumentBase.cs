//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain.ArgumentParsing
{
    public class BookGenArgumentBase : ArgumentsBase
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
