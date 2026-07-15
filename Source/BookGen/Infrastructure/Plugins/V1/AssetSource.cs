using System.Diagnostics.CodeAnalysis;

using BookGen.Lib;

namespace BookGen.Infrastructure.Plugins.V1;

internal sealed class AssetSource : Api.V1.IAssetSource
{
    private readonly BookEnvironment _bookEnvironment;

    public AssetSource(BookEnvironment bookEnvironment)
    {
        _bookEnvironment = bookEnvironment;
    }

    public IReadOnlyList<string> AvailableAssets
        => _bookEnvironment.AssetNames;

    public Stream GetBinaryAssetStream(string name)
        => _bookEnvironment.GetBinaryAssetStream(name);

    public bool TryGetAsset(string name, [NotNullWhen(true)] out string? content)
        => _bookEnvironment.TryGetAsset(name, out content);
}
