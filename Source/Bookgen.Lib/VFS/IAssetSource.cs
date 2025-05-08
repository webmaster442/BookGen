using System.Diagnostics.CodeAnalysis;

namespace Bookgen.Lib.VFS;

public interface IAssetSource
{
    bool TryGetAsset(string name, [MaybeNullWhen(false)] out string? content);
}