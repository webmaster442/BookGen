//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using BookGen.Infrastructure;
using BookGen.Lib;
using BookGen.Lib.Domain.IO;
using BookGen.Lib.Domain.IO.Configuration;
using BookGen.Lib.Pipeline;

using BookGen.Vfs;

using Moq;

namespace Bookgen.Tests;

internal class TestEnvironment : IBookEnvironment
{
    public Config Configuration => throw new NotImplementedException();

    public TableOfContents TableOfContents => throw new NotImplementedException();

    public IWritableFileSystem Source => throw new NotImplementedException();

    public IWritableFileSystem Output => throw new NotImplementedException();

    public IReadOnlyList<string> AssetNames => _assetSoruce.AssetNames;

    public IProgramPathResolver ProgramPathResolver { get; }

    public void Dispose()
    {
        _assetSoruce.Dispose();
    }

    private readonly ZipAssetSoruce _assetSoruce;

    public TestEnvironment()
    {
        _assetSoruce = new ZipAssetSoruce(Path.Combine(AppContext.BaseDirectory, "assets.zip"));
        ProgramPathResolver = new ProgramPathResolver(new BookGen.Cli.Dotenv.DotEnvSettings());
    }

    public bool TryGetAsset(string name, [NotNullWhen(true)] out string? content)
    {
        return _assetSoruce.TryGetAsset(name, out content);
    }

    public static bool IsBookGenFolder(string folder)
        => false;

    public Stream GetBinaryAssetStream(string name)
        => _assetSoruce.GetBinaryAssetStream(name);
}
