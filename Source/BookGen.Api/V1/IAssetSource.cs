using System.Diagnostics.CodeAnalysis;

namespace BookGen.Api.V1;

/// <summary>
/// Represents a source of assets that can be used by the Bookgen application.
/// </summary>
public interface IAssetSource
{
    /// <summary>
    /// Gets a read-only list of available asset names provided by this asset source.
    /// </summary>
    IReadOnlyList<string> AvailableAssets { get; }
    /// <summary>
    /// Attempts to retrieve the content of an asset by its name. Returns true if the asset is found, otherwise false.
    /// </summary>
    /// <param name="name">The name of the asset to retrieve.</param>
    /// <param name="content">When this method returns, contains the content of the asset if found; otherwise, null.</param>
    /// <returns>True if the asset is found; otherwise, false.</returns>
    bool TryGetAsset(string name, [NotNullWhen(true)] out string? content);
    /// <summary>
    /// Gets a stream for reading the binary content of an asset by its name.
    /// </summary>
    /// <param name="name">The name of the asset to retrieve.</param>
    /// <returns>A stream for reading the binary content of the asset.</returns>
    Stream GetBinaryAssetStream(string name);
}
