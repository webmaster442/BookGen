//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;

namespace WpLoad.Domain
{
    internal class UpArguments : ArgumentsBase
    {
        [Switch("p", "path")]
        public string Path { get; set; }

        [Switch("s", "Site", true)]
        public string Site { get; set; }

        public UpArguments()
        {
            Path = Environment.CurrentDirectory;
            Site = string.Empty;
        }
    }
}
