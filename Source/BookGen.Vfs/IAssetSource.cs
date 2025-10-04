//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace BookGen.Vfs;

public interface IAssetSource
{
    bool TryGetAsset(string name, [NotNullWhen(true)] out string? content);

    IReadOnlyList<string> AssetNames { get; }

    byte[] GetBinaryAsset(string name);

    string GetAsset(string name)
    {
        if (!TryGetAsset(name, out var asset))
        {
            throw new InvalidOperationException($"{name} was not found in assets");
        }
        return asset;
    }
}