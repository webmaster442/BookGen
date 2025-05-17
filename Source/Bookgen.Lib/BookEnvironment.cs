using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

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

    private Config? _config;
    private TableOfContents? _toc;
    private IWritableFileSystem? _output;
    private bool _isInitialized;

    public BookEnvironment(IWritableFileSystem soruceFolder, params IAssetSource[] assets)
    {
        _source = soruceFolder;
        _assets = assets;
        Cache = new Cache();
    }

    public Config Configuration => _config ?? throw new InvalidOperationException();

    public TableOfContents TableOfContents => _toc ?? throw new InvalidOperationException();

    public IWritableFileSystem Source => _isInitialized ? _source : throw new InvalidOperationException();

    public IWritableFileSystem Output => _output ?? throw new InvalidOperationException();

    public ICache Cache { get; }

    public static bool IsBookGenFolder(string folder)
        => File.Exists(Path.Combine(folder, Constants.ConfigFile));

    public void Dispose()
    {
        Cache.Clear();

        foreach (var asset in _assets)
        {
            if (asset is IDisposable disposable)
                disposable.Dispose();
        }

        if (_isInitialized && _source.FileExists(Constants.LockFile))
        {
            _source.Delete(Constants.LockFile);
        }
    }

    public async Task<EnvironmentStatus> Initialize()
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException();
        }

        EnvironmentStatus status = new EnvironmentStatus();

        if (!_source.FileExists(Constants.ConfigFile))
        {
            status.Add($"No {Constants.ConfigFile} found in folder {_source.Scope}");
            return status;
        }

        Config? config = await _source.DeserializeAsync<Config>(Constants.ConfigFile);

        if (config == null)
        {
            status.Add("Config file load failed");
            return status;
        }

        Config defaultConfig = new();

        if (config.VersionTag < defaultConfig.VersionTag)
        {
            await _source.SerializeAsync(Constants.ConfigFile, config);
            status.Add($"Config from version {config.VersionTag} was updated to {defaultConfig.VersionTag}. Check settings and re-execute");
            return status;
        }

        if (!SerializedObjectValidator.Validate(config, _source, status))
        {
            return status;
        }

        if (!_source.FileExists(config.TocFile))
        {
            status.Add($"No {config.TocFile} found in folder {_source.Scope}");
            return status;
        }

        TableOfContents? toc = await _source.DeserializeAsync<TableOfContents>(config.TocFile);
        if (toc == null)
        {
            status.Add($"{config.TocFile} file load failed");
            return status;
        }

        if (!SerializedObjectValidator.Validate(toc, _source, status))
        {
            return status;
        }

        _config = config;
        _toc = toc;

        using Process currentProcess = Process.GetCurrentProcess();

        if (!_source.FileExists(Constants.LockFile))
        {
            var id = currentProcess.Id.ToString();
            await _source.WriteAllTextAsync(Constants.LockFile, id);
        }
        else
        {
            if (!int.TryParse(_source.ReadAllText(Constants.LockFile), out int id))
            {
                id = -1;
            }
            if (currentProcess.Id == id)
            {
                status.Add($"{_source.Scope} is locked by an other running BookGen process");
                return status;
            }
            else
            {
                _source.Delete(Constants.LockFile);
            }
        }

        _isInitialized = true;
        _output = new FileSystem(config.OutputFolder);
        Cache.Clear();

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
