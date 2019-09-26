//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Core.Configuration
{
    public sealed class Asset
    {
        [Doc("path relative to input directory", true)]
        public string Source
        {
            get;
            set;
        }

        [Doc("path relative to output directory", true)]
        public string Target
        {
            get;
            set;
        }
    }
}
