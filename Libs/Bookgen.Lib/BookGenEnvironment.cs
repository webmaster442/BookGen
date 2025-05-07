using System.Diagnostics;

using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.Internals;
using Bookgen.Lib.Pipeline;
using Bookgen.Lib.VFS;

namespace Bookgen.Lib;

internal static class Constants
{
    public const string ConfigFile = "bookgen.json";
    public const string LockFile = "bookgen.lock";
}

public sealed class BookGenEnvironment : IEnvironment
{
    private readonly IFolder _folder;
    private Config? _config;
    private TableOfContents? _toc;
    private bool _isInitialized;

    public BookGenEnvironment(IFolder soruceFolder)
    {
        _folder = soruceFolder;
    }

    public Config Configuration => _config ?? throw new InvalidOperationException();

    public TableOfContents TableOfContents => _toc ?? throw new InvalidOperationException();

    public IFolder Source => _isInitialized ? _folder : throw new InvalidOperationException();

    public void Dispose()
    {
        if (_isInitialized && _folder.Exists(Constants.LockFile))
        {
            _folder.Delete(Constants.LockFile);
        }
    }

    public async Task<EnvironmentStatus> Initialize()
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException();
        }

        EnvironmentStatus status = new EnvironmentStatus();

        if (!_folder.Exists(Constants.ConfigFile))
        {
            status.Add($"No {Constants.ConfigFile} found in folder {_folder.FullPath}");
            return status;
        }

        Config? config = await _folder.DeserializeAsync<Config>(Constants.ConfigFile);

        if (config == null)
        {
            status.Add("Config file load failed");
            return status;
        }

        Config defaultConfig = Config.GetDefault();

        if (config.VersionTag < defaultConfig.VersionTag)
        {
            await _folder.SerializeAsync(Constants.ConfigFile, config);
            status.Add($"Config from version {config.VersionTag} was updated to {defaultConfig.VersionTag}. Check settings and re-execute");
            return status;
        }

        if (!SerializedObjectValidator.Validate(config, _folder, status))
        {
            return status;
        }

        if (!_folder.Exists(config.TocFile))
        {
            status.Add($"No {config.TocFile} found in folder {_folder.FullPath}");
            return status;
        }

        TableOfContents? toc = await _folder.DeserializeAsync<TableOfContents>(config.TocFile);
        if (toc == null)
        {
            status.Add($"{config.TocFile} file load failed");
            return status;
        }

        if (!SerializedObjectValidator.Validate(toc, _folder, status))
        {
            return status;
        }

        _config = config;
        _toc = toc;

        if (!_folder.Exists(Constants.LockFile))
        {
            var id = Process.GetCurrentProcess().Id.ToString();
            await _folder.WriteTextAsync(Constants.LockFile, id);
        }

        _isInitialized = true;

        return status;
    }
}
