using System.Diagnostics.CodeAnalysis;

using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.Pipeline;

using BookGen.Vfs;

namespace Bookgen.Tests;
internal class TestEnvironment : IEnvironment
{
    public Config Configuration => throw new NotImplementedException();

    public TableOfContents TableOfContents => throw new NotImplementedException();

    public IWritableFileSystem Source => throw new NotImplementedException();

    public IWritableFileSystem Output => throw new NotImplementedException();

    public ICache Cache => throw new NotImplementedException();

    public void Dispose()
    {
        _assetSoruce.Dispose();
    }

    private readonly ZipAssetSoruce _assetSoruce;

    public TestEnvironment()
    {
        _assetSoruce = new ZipAssetSoruce(Path.Combine(AppContext.BaseDirectory, "assets.zip"));
    }

    public bool TryGetAsset(string name, [NotNullWhen(true)] out string? content)
    {
        return _assetSoruce.TryGetAsset(name, out content);
    }
}
