//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Domain;
using BookGen.Interfaces;
using BookGen.RakeEngine;
using System.Globalization;

namespace BookGen.DomainServices
{
    public class WritableTagUtils : TagUtils
    {
        private readonly ILog _log;

        public WritableTagUtils(Dictionary<string, string[]> loadedTags, CultureInfo culture, ILog log)
            : base(loadedTags, culture)
        {
            _log = log;
        }

        public void DeleteNoLongerExisting(ToC toc)
        {
            _log.Info("Scanning no longer existing entries...");
            var tocItems = toc.Files.ToHashSet();
            Stack<string> toDelete = new();
            foreach (string? file in _loadedTags.Keys)
            {
                if (!tocItems.Contains(file))
                    toDelete.Push(file);
            }

            _log.Info("Found {0} items to remove...", toDelete.Count);
            while (toDelete.Count > 0)
            {
                _loadedTags.Remove(toDelete.Pop());
            }
        }

        public void CreateNotYetExisting(ToC toc)
        {
            _log.Info("Scanning not yet existing entries...");
            int count = 0;
            foreach (string? file in toc.Files)
            {
                if (!_loadedTags.ContainsKey(file))
                {
                    _loadedTags.Add(file, Array.Empty<string>());
                    ++count;
                }
            }
            _log.Info("Created {0} new tag entries. Please fill them.", count);
        }

        public void AutoGenerate(ToC toc, int keywordCount)
        {
            _log.Info("Auto Generating tags...");
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
