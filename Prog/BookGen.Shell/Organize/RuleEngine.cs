//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.RegularExpressions;

using BookGen.Api;
using BookGen.Domain.Organize;

namespace BookGen.Shell.Organize;
internal class RuleEngine
{
    private readonly Dictionary<Regex, string> _loadedRules;
    private readonly ILog _log;

    public RuleEngine(IReadOnlyList<OrganizeRule> rules, ILog log)
    {
        _loadedRules = rules.ToDictionary(rule => rule.GetRegex(), rule => rule.Destination);
        _log = log;
    }

    public void Run(string folder, bool simulate)
    {
        foreach (var file in Directory.GetFiles(folder))
        {
            var foundRule = _loadedRules.FirstOrDefault(rule => rule.Key.IsMatch(file));
            var newName = Path.Combine(foundRule.Value, Path.GetFileName(file));
            if (!simulate)
            {
                if (!Directory.Exists(foundRule.Value))
                {
                    Directory.CreateDirectory(foundRule.Value);
                }
                File.Move(file, newName);
            }
            _log.Info($"{file} => {newName}");
        }
    }

}
