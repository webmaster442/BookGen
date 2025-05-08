using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.Internals;
using Bookgen.Lib.Pipeline;
using Bookgen.Lib.VFS;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib;

internal static class Constants
{
    public const string ConfigFile = "bookgen.json";
    public const string LockFile = "bookgen.lock";
}

public sealed class BookGenEnvironment : IEnvironment
{
    private readonly IFolder _source;
    private readonly IAssetSource[] _assets;

    private Config? _config;
    private TableOfContents? _toc;
    private IFolder? _output;
    private bool _isInitialized;

    public BookGenEnvironment(IFolder soruceFolder, params IAssetSource[] assets)
    {
        _source = soruceFolder;
        _assets = assets;
    }

    public Config Configuration => _config ?? throw new InvalidOperationException();

    public TableOfContents TableOfContents => _toc ?? throw new InvalidOperationException();

    public IFolder Source => _isInitialized ? _source : throw new InvalidOperationException();

    public IFolder Output => _output ?? throw new InvalidOperationException();

    public void Dispose()
    {
        foreach (var asset in _assets)
        {
            if (asset is IDisposable disposable)
                disposable.Dispose();
        }

        if (_isInitialized && _source.Exists(Constants.LockFile))
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

        if (!_source.Exists(Constants.ConfigFile))
        {
            status.Add($"No {Constants.ConfigFile} found in folder {_source.FullPath}");
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

        if (!_source.Exists(config.TocFile))
        {
            status.Add($"No {config.TocFile} found in folder {_source.FullPath}");
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

        if (!_source.Exists(Constants.LockFile))
        {
            var id = currentProcess.Id.ToString();
            await _source.WriteTextAsync(Constants.LockFile, id);
        }
        else
        {
            if (!int.TryParse(_source.ReadText(Constants.LockFile), out int id))
            {
                id = -1;
            }
            if (currentProcess.Id == id)
            {
                status.Add($"{_source.FullPath} is locked by an other running BookGen process");
                return status;
            }
            else
            {
                _source.Delete(Constants.LockFile);
            }
        }

        _isInitialized = true;
        _output = new FileSystemFolder(config.OutputFolder);

        return status;
    }

    public bool TryGetAsset(string name, [MaybeNullWhen(false)] out string? content)
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
