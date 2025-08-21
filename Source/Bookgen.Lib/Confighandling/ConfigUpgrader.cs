using System.Text.Json.Nodes;

using Bookgen.Lib.Domain.IO.Configuration;

using BookGen.Vfs;

namespace Bookgen.Lib.Confighandling;

internal class ConfigUpgrader
{
    private readonly UpgradeBase[] _upgrades;
    private JsonObject? _tocJson;
    private JsonObject? _configJson;
    private int _sourceVersion;

    public int VersionTag => _sourceVersion;

    public bool NeedsUpgrade { get; private set; }

    public ConfigUpgrader()
    {
        _upgrades = typeof(UpgradeBase).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && t.IsClass)
            .Where(t => t.IsAssignableTo(typeof(UpgradeBase)))
            .Select(t => Activator.CreateInstance(t))
            .OfType<UpgradeBase>()
            .OrderBy(u => u.VersionTagInfo)
            .ToArray();
    }

    public async Task Init(IReadOnlyFileSystem sourceFolder)
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
            NeedsUpgrade = true;
        }
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

            JsonObject sorted = new JsonObject(_configJson.OrderBy(k => k.Key));
           
            sourceFolder.MoveFile(FileNameConstants.ConfigFile, FileNameConstants.ConfigFile + ".bak");
            await sourceFolder.WriteJsonAsync(FileNameConstants.ConfigFile, sorted);
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
