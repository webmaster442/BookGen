using System.Diagnostics.CodeAnalysis;

namespace BookGen.Vfs;

public interface IAssetSource
{
    bool TryGetAsset(string name, [NotNullWhen(true)] out string? content);

    string GetAsset(string name)
    {
        if (!TryGetAsset(name, out var asset))
        {
            throw new InvalidOperationException($"{name} was not found in assets");
        }
        return asset;
    }
}