//-----------------------------------------------------------------------------
// (c) 2022-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Globalization;

using BookGen.Domain;
using BookGen.Interfaces;
using BookGen.RakeEngine;

using Microsoft.Extensions.Logging;

namespace BookGen.DomainServices
{
    public class WritableTagUtils : TagUtils
    {
        private readonly ILogger _log;

        public WritableTagUtils(Dictionary<string, string[]> loadedTags, CultureInfo culture, ILogger log)
            : base(loadedTags, culture)
        {
            _log = log;
        }

        public void DeleteNoLongerExisting(ToC toc)
        {
            _log.LogInformation("Scanning no longer existing entries...");
            var tocItems = toc.Files.ToHashSet();
            Stack<string> toDelete = new();
            foreach (string? file in _loadedTags.Keys)
            {
                if (!tocItems.Contains(file))
                    toDelete.Push(file);
            }

            _log.LogInformation("Found {count} items to remove...", toDelete.Count);
            while (toDelete.Count > 0)
            {
                _loadedTags.Remove(toDelete.Pop());
            }
        }

        public void CreateNotYetExisting(ToC toc)
        {
            _log.LogInformation("Scanning not yet existing entries...");
            int count = 0;
            foreach (string? file in toc.Files)
            {
                if (!_loadedTags.ContainsKey(file))
                {
                    _loadedTags.Add(file, Array.Empty<string>());
                    ++count;
                }
            }
            _log.LogInformation("Created {count} new tag entries. Please fill them.", count);
        }

        public void AutoGenerate(ToC toc, int keywordCount)
        {
            _log.LogInformation("Auto Generating tags...");
            Rake rake = new Rake(
                stopWordCulture: _culture,
                minCharLength: 3,
                maxWordsLength: 1,
                minKeywordFrequency: 1);

            foreach (string? file in toc.Files)
            {
                if (_loadedTags.ContainsKey(file))
                {
                    string mdContent = new FsPath(file).ReadFile(_log);

                    var keywords = rake
                        .RunMarkdown(mdContent)
                        .Take(keywordCount)
                        .Select(x => x.Key);

                    _loadedTags[file] = keywords.Distinct().ToArray();
                }
            }
        }
    }
}
