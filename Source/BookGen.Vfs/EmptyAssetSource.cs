//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace BookGen.Vfs;

public sealed class EmptyAssetSource : IAssetSource
{
    public IReadOnlyList<string> AssetNames
        => Array.Empty<string>();

    public Stream GetBinaryAssetStream(string name)
    {
        return Stream.Null;
    }

    public bool TryGetAsset(string name, [NotNullWhen(true)] out string? content)
    {
        content = null;
        return false;
    }
}
