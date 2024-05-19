//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Text.Json;

using BookGen.Domain.Organize;

namespace BookGen.Shell.Organize;

internal sealed class RuleLoader
{
    private readonly string _ruleFile;
    private readonly JsonSerializerOptions _options;
    private const string RuleName = ".organize";

    public RuleLoader(string folder)
    {
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _ruleFile = Path.Combine(folder, RuleName);
    }

    public IReadOnlyList<OrganizeRule> LoadRules()
    {
        if (File.Exists(_ruleFile))
        {
            return Deserialize();
        }
        else
        {
            CreateDefaultFile();
            EditFile();
            return Deserialize();
        }
    }

    private void CreateDefaultFile()
    {
        var collection = JsonSerializer.Serialize(DefaultRulesProvider.Defaults.ToArray(), _options);
        File.WriteAllText(_ruleFile, collection);
    }

    private void EditFile()
    {
        using var process = new Process();
        process.StartInfo.FileName = _ruleFile;
        process.StartInfo.UseShellExecute = true;
        process.Start();
        process.WaitForExit();
    }

    private OrganizeRule[] Deserialize()
    {
        var collection = JsonSerializer.Deserialize<OrganizeRule[]>(File.ReadAllText(_ruleFile), _options);
        return collection?.Length > 0 ? collection : [];
    }
}
