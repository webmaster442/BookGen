using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

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
    private readonly IWritableFileSystem _output;
    private readonly IAssetSource[] _assets;
    private readonly ConfigUpgrader _configUpgrader;
    
    private FolderLock? _folderLock;
    private Config? _config;
    private TableOfContents? _toc;
    
    private bool _isInitialized;

    public BookEnvironment(IWritableFileSystem soruceFolder, IWritableFileSystem output, params IAssetSource[] assets)
    {
        _source = soruceFolder;
        _output = output;
        _assets = assets;
        _configUpgrader = new ConfigUpgrader();
    }

    const string Error = $"{nameof(Initialize)} was not called";

    public Config Configuration => _config ?? throw new InvalidOperationException(Error);

    public TableOfContents TableOfContents => _toc ?? throw new InvalidOperationException(Error);

    public IWritableFileSystem Source => _isInitialized ? _source : throw new InvalidOperationException(Error);

    public IWritableFileSystem Output => _isInitialized ? _output : throw new InvalidOperationException(Error);

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

    public async Task<EnvironmentStatus> Initialize(bool autoUpgrade)
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

        await _configUpgrader.Init(_source);

        if(_configUpgrader.NeedsUpgrade && autoUpgrade)
        {
            (bool tocModified, bool configmodified) = await _configUpgrader.Upgrade(_source);
            status.Add($"Config from version {_configUpgrader.VersionTag} was updated to {Config.CurrentVersionTag}. Check settings and re-execute");
            if (tocModified)
                await _source.WriteSchema<TableOfContents>(FileNameConstants.TableOfContentsSchema);
            if (configmodified)
                await _source.WriteSchema<Config>(FileNameConstants.ConfigFileSchema);

            return status;
        }

        Config? config = await _source.DeserializeAsync<Config>(FileNameConstants.ConfigFile);

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

        _config = config;
        _toc = toc;

        _folderLock = new FolderLock(_source, FileNameConstants.LockFile);

        _isInitialized = _folderLock.Initialize();
        return status;
    }

    public bool TryGetAsset(string name, [NotNullWhen(true)] out string? content)
    {
        foreach (var assetsource in _assets)
        {
            if (assetsource.TryGetAsset(name, out content))
            {
                return true;
            }
        }

        content = null;
        return false;
    }
}
