//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using Bookgen.Lib.AppSettings;
using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.Pipeline;

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
    private readonly Mock<IReadOnlyAppSettings> _appSettingsMock;

    public TestEnvironment()
    {
        _appSettingsMock = new Mock<IReadOnlyAppSettings>(MockBehavior.Strict);
        _appSettingsMock.Setup(x => x.Get(x => x.NodeJsPath)).Returns((string?)null);
        _appSettingsMock.Setup(x => x.Get(x => x.PythonPath)).Returns((string?)null);
        _appSettingsMock.Setup(x => x.Get(x => x.RatexPath)).Returns((string?)null);
        _appSettingsMock.Setup(x => x.Get(x => x.MmdrPath)).Returns((string?)null);
        _appSettingsMock.Setup(x => x.Get(x => x.PlantUmlPath)).Returns((string?)null);
        _assetSoruce = new ZipAssetSoruce(Path.Combine(AppContext.BaseDirectory, "assets.zip"));
        ProgramPathResolver = new ProgramPathResolver(_appSettingsMock.Object);
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
