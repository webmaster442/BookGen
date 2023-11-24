//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces.Configuration;

namespace BookGen.Domain.Configuration
{
    public sealed class Asset : IReadOnlyAsset
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

        public Asset()
        {
            Source = string.Empty;
            Target = string.Empty;
        }
    }
}
