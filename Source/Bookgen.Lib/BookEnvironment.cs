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
    }

    public Config Configuration => _config ?? throw new InvalidOperationException();

    public TableOfContents TableOfContents => _toc ?? throw new InvalidOperationException();

    public IWritableFileSystem Source => _isInitialized ? _source : throw new InvalidOperationException();

    public IWritableFileSystem Output => _output ?? throw new InvalidOperationException();

    public static bool IsBookGenFolder(string folder)
        => File.Exists(Path.Combine(folder, FileNameConstants.ConfigFile));

    public void Dispose()
    {
        Cache.Clear();

        foreach (var asset in _assets)
        {
            if (asset is IDisposable disposable)
                disposable.Dispose();
        }

        if (_isInitialized && _source.FileExists(FileNameConstants.LockFile))
        {
            _source.Delete(FileNameConstants.LockFile);
        }
    }

    public async Task<EnvironmentStatus> Initialize()
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

        Config? config = await _source.DeserializeAsync<Config>(FileNameConstants.ConfigFile);

        if (config == null)
        {
            status.Add("Config file load failed");
            return status;
        }

        Config defaultConfig = new();

        if (config.VersionTag < defaultConfig.VersionTag)
        {
            await _source.SerializeAsync(FileNameConstants.ConfigFile, config);
            status.Add($"Config from version {config.VersionTag} was updated to {defaultConfig.VersionTag}. Check settings and re-execute");
            return status;
        }

        if (!validator.Validate(config, status))
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

        if (!validator.Validate(toc, status))
        {
            return status;
        }

        _config = config;
        _toc = toc;

        using Process currentProcess = Process.GetCurrentProcess();

        if (!_source.FileExists(FileNameConstants.LockFile))
        {
            var id = currentProcess.Id.ToString();
            await _source.WriteAllTextAsync(FileNameConstants.LockFile, id);
        }
        else
        {
            if (!int.TryParse(_source.ReadAllText(FileNameConstants.LockFile), out int id))
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
                _source.Delete(FileNameConstants.LockFile);
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
