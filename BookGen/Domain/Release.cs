//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen.Domain
{
    public record Release
    {
        public Version Version { get; init; }
        public string ZipPackageUrl { get; init; }
        public string HashSha256 { get; init; }
        public bool IsPreview { get; init; }

        public Release()
        {
            Version = new Version();
            ZipPackageUrl = string.Empty;
            HashSha256 = string.Empty;
        }
    }
}
