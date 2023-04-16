//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Update.Dto;

public sealed record Release
{
    public string Version { get; init; }
    public string ZipPackageUrl { get; init; }
    public string HashSha256 { get; init; }
    public bool IsPreview { get; init; }

    public Release()
    {
        Version = string.Empty;
        ZipPackageUrl = string.Empty;
        HashSha256 = string.Empty;
    }
}
