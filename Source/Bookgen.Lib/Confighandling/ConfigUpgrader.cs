//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Nodes;

using Bookgen.Lib.Domain.IO.Configuration;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Confighandling;

public sealed class ConfigUpgrader
{
    private readonly UpgradeBase[] _upgrades;
    private readonly ILogger _logger;
    private JsonObject? _tocJson;
    private JsonObject? _configJson;
    private int _sourceVersion;

    public int VersionTag => _sourceVersion;

    public ConfigUpgrader(ILogger logger)
    {
        _upgrades = typeof(UpgradeBase).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && t.IsClass)
            .Where(t => t.IsAssignableTo(typeof(UpgradeBase)))
            .Select(t => Activator.CreateInstance(t))
            .OfType<UpgradeBase>()
            .OrderBy(u => u.VersionTagInfo)
            .ToArray();
        _logger = logger;
    }

    public async Task<bool> Init(IReadOnlyFileSystem sourceFolder)
    {
        _tocJson = await sourceFolder.ReadJsonAsync(FileNameConstants.TableOfContents);
        _configJson = await sourceFolder.ReadJsonAsync(FileNameConstants.ConfigFile);

        var version = _configJson["VersionTag"]
            ?? throw new InvalidOperationException("Failed to determine version");

        _sourceVersion = 0;

        if (version is JsonValue jsonValue)
        {
            _sourceVersion = jsonValue.GetValue<int>();
        }

        if (_sourceVersion < Config.CurrentVersionTag)
        {
            return true;
        }

        return false;
    }

    public static async Task<bool> IsUpgradeNeeded(IReadOnlyFileSystem sourceFolder)
    {
        var configJson = await sourceFolder.ReadJsonAsync(FileNameConstants.ConfigFile);

        var version = configJson["VersionTag"]
            ?? throw new InvalidOperationException("Failed to determine version");

        if (version is not JsonValue jsonValue
            || !jsonValue.TryGetValue<int>(out int sourceVersion))
        {
            throw new InvalidOperationException("Failed to determine version");
        }
        return sourceVersion < Config.CurrentVersionTag;
    }

    public async Task<(bool tocModifed, bool configModified)> Upgrade(IWritableFileSystem sourceFolder)
    {
        if (_tocJson == null)
            throw new InvalidOperationException("TOC JSON is not initialized");

        if (_configJson == null)
            throw new InvalidOperationException("Config JSON is not initialized");

        var info = new VersionTagInfo(_sourceVersion, Config.CurrentVersionTag);

        List<UpgradeBase> upgraders = SelectUpgrades(info.From, info.To);


        if (upgraders.Count == 0)
        {
            throw new InvalidOperationException($"No upgrade path found from version {info.From} to {info.To}");
        }

        bool tocModifed = false;
        bool configModified = false;

        foreach (var upgrader in upgraders)
        {
            _logger.LogInformation("Upgrading from version {from} to {to}", upgrader.VersionTagInfo.From, upgrader.VersionTagInfo.To);
            tocModifed |= upgrader.UpgradeToc(_tocJson);
            configModified |= upgrader.UpgradeConfig(_configJson);
        }

        if (tocModifed)
        {
            sourceFolder.MoveFile(FileNameConstants.TableOfContents, FileNameConstants.TableOfContents + ".bak");
            await sourceFolder.WriteJsonAsync(FileNameConstants.TableOfContents, _tocJson);
        }

        if (configModified)
        {
            _configJson["VersionTag"] = info.To;
           
            sourceFolder.MoveFile(FileNameConstants.ConfigFile, FileNameConstants.ConfigFile + ".bak");
            await sourceFolder.WriteJsonAsync(FileNameConstants.ConfigFile, _configJson);
        }

        return (tocModifed, configModified);
    }

    private List<UpgradeBase> SelectUpgrades(int from, int to)
    {
        List<UpgradeBase> selectedUpgrades = new List<UpgradeBase>();
        foreach (var upgrader in _upgrades)
        {
            if (upgrader.VersionTagInfo.From >= from && upgrader.VersionTagInfo.To <= to)
            {
                selectedUpgrades.Add(upgrader);
            }
        }
        return selectedUpgrades;
    }
}
