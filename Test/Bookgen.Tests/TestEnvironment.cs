using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.Pipeline;
using Bookgen.Lib.VFS;

namespace Bookgen.Tests;
internal class TestEnvironment : IEnvironment
{
    public Config Configuration => throw new NotImplementedException();

    public TableOfContents TableOfContents => throw new NotImplementedException();

    public IFolder Source => throw new NotImplementedException();

    public IFolder Output => throw new NotImplementedException();

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
