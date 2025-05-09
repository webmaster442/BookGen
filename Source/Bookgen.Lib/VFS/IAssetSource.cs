using System.Diagnostics.CodeAnalysis;

namespace Bookgen.Lib.VFS;

public interface IAssetSource
{
    bool TryGetAsset(string name, [NotNullWhen(true)] out string? content);
}