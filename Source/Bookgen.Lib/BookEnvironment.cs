//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

using Bookgen.Lib.AppSettings;
using Bookgen.Lib.Confighandling;
using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.Internals;
using Bookgen.Lib.Pipeline;

using BookGen.Vfs;

namespace Bookgen.Lib;

public sealed class BookEnvironment : IBookEnvironment
{
    private readonly IWritableFileSystem _source;
    private readonly IAssetSource[] _assets;

    private FolderLock? _folderLock;
    private bool _isInitialized;

    public BookEnvironment(IWritableFileSystem soruceFolder,
                           IWritableFileSystem output,
                           IProgramPathResolver programPathResolver,
                           params IAssetSource[] assets)
    {
        _source = soruceFolder;
        Output = output;
        _assets = assets;
        ProgramPathResolver = programPathResolver;
        AssetNames = assets.SelectMany(a => a.AssetNames).Distinct().ToArray();
    }

    const string Error = $"{nameof(Initialize)} was not called";

    public Config Configuration { get => field ?? throw new InvalidOperationException(Error); private set; }

    public TableOfContents TableOfContents { get => field ?? throw new InvalidOperationException(Error); private set; }

    public IWritableFileSystem Source => _isInitialized ? _source : throw new InvalidOperationException(Error);

    public IWritableFileSystem Output => _isInitialized ? field : throw new InvalidOperationException(Error);

    public IProgramPathResolver ProgramPathResolver => _isInitialized ? field : throw new InvalidOperationException(Error);

    public IReadOnlyList<string> AssetNames { get; }

    public static bool IsBookGenFolder(string folder)
        => File.Exists(Path.Combine(folder, FileNameConstants.ConfigFile));

    public void Dispose()
    {
        if (_folderLock != null)
        {
            _folderLock.Dispose();
            _folderLock = null;
        }
    }

    public async Task<EnvironmentStatus> Initialize(string? configOverlay)
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException();
        }

        var validator = new SerializedObjectValidator(_source);

        EnvironmentStatus status = new EnvironmentStatus();

        if (!_source.FileExists(FileNameConstants.ConfigFile))
        {
            status.Add($"No {FileNameConstants.ConfigFile} found in folder {_source.Scope}");
            return status;
        }

        if (!_source.FileExists(FileNameConstants.TableOfContents))
        {
            status.Add($"No {FileNameConstants.TableOfContents} found in folder {_source.Scope}");
            return status;
        }

        bool updateNeeded = await ConfigUpgrader.IsUpgradeNeeded(_source);
        if (updateNeeded)
        {
            status.Add("Config file is too old. Run bookgen upgrade to update it");
            return status;
        }

        JsonObject baseConfig = await _source.ReadJsonAsync(FileNameConstants.ConfigFile);

        JsonMerger configMerger = new JsonMerger(baseConfig);

        if (!string.IsNullOrEmpty(configOverlay))
        {
            JsonObject overlayConfig = await _source.ReadJsonAsync(configOverlay);
            configMerger.Merge(overlayConfig);
        }

        Config? config = configMerger.Deserialize<Config>();

        if (config == null)
        {
            status.Add("Config file load failed");
            return status;
        }

        if (!validator.Validate(config, status))
        {
            return status;
        }

        TableOfContents? toc = await _source.DeserializeAsync<TableOfContents>(FileNameConstants.TableOfContents);
        if (toc == null)
        {
            status.Add($"{FileNameConstants.TableOfContents} file load failed");
            return status;
        }

        if (!validator.Validate(toc, status))
        {
            return status;
        }

        Configuration = config;
        TableOfContents = toc;

        _folderLock = new FolderLock(_source, FileNameConstants.LockFile);

        _isInitialized = _folderLock.Initialize();
        return status;
    }

    public bool TryGetAsset(string name, [NotNullWhen(true)] out string? content)
    {
        foreach (IAssetSource assetsource in _assets)
        {
            if (assetsource.TryGetAsset(name, out content))
            {
                return true;
            }
        }

        content = null;
        return false;
    }

    public Stream GetBinaryAssetStream(string name)
    {
        foreach (IAssetSource assetsource in _assets)
        {
            try
            {
                return assetsource.GetBinaryAssetStream(name);
            }
            catch (InvalidOperationException)
            {
                // Ignore and try next asset source
            }
        }
        throw new InvalidOperationException($"{name} was not found in assets");
    }
}
