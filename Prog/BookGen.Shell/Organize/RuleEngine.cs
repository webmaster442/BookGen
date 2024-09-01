//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.RegularExpressions;

using BookGen.Domain.Organize;

using Microsoft.Extensions.Logging;

namespace BookGen.Shell.Organize;
internal class RuleEngine
{
    private readonly Dictionary<Regex, string> _loadedRules;
    private readonly ILogger _log;

    public RuleEngine(IReadOnlyList<OrganizeRule> rules, ILogger log)
    {
        _loadedRules = rules.ToDictionary(rule => rule.GetRegex(), rule => rule.Destination);
        _log = log;
    }

    public void Run(string folder, bool simulate)
    {
        foreach (var file in Directory.GetFiles(folder))
        {
            var foundRule = _loadedRules.FirstOrDefault(rule => rule.Key.IsMatch(file));

            if (foundRule.Key == null)
            {
                _log.LogWarning("No rule found for {file}", file);
                continue;
            }

            var newName = Path.Combine(foundRule.Value, Path.GetFileName(file));
            if (!simulate)
            {
                if (!Directory.Exists(foundRule.Value))
                {
                    Directory.CreateDirectory(foundRule.Value);
                }
                File.Move(file, newName);
            }
            _log.LogInformation("{file} => {newName}", file, newName);
        }
    }

}
