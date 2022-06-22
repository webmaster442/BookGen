//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.ArgumentParser;

namespace WpLoad.Domain
{
    internal class DownArguments : UpArguments
    {
        [Switch("p", "posts")]
        public bool Posts { get; set; }
        [Switch("pg", "pages")]
        public bool Pages { get; set; }
        [Switch("m", "media")]
        public bool Media { get; set; }
    }
}
