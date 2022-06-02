//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core.Contracts;
using BookGen.Domain;
using System.Globalization;

namespace BookGen.Utilities
{
    internal class TagUtils : ITagUtils
    {
        private readonly Dictionary<string, string[]> _loadedTags;
        private readonly ILog? _log;
        private readonly CultureInfo _culture;

        public Dictionary<string, string[]> TagCollection => _loadedTags;

        public int UniqueTagCount
            => _loadedTags.SelectMany(x => x.Value).DistinctBy(x => x.ToLower()).Count();

        public int TotalTagCount
            => _loadedTags.SelectMany(x => x.Value).Count();

        public int FilesWithOutTags
            => _loadedTags.Where(x => x.Value == null || x.Value.Length < 1).Count();

        public TagUtils()
        {
            _loadedTags = new();
            _culture = CultureInfo.InvariantCulture;
        }

        public TagUtils(Dictionary<string, string[]> loadedTags, CultureInfo culture, ILog log)
        {
            _loadedTags = loadedTags;
            _log = log;
            _culture = culture;
        }

        public void DeleteNoLongerExisting(ToC toc)
        {
            _log?.Info("Scanning no longer existing entries...");
            var tocItems = toc.Files.ToHashSet();
            Stack<string> toDelete = new();
            foreach (var file in _loadedTags.Keys)
            {
                if (!tocItems.Contains(file))
                    toDelete.Push(file);
            }

            _log?.Info("Found {0} items to remove...", toDelete.Count);
            while (toDelete.Count > 0)
            {
                _loadedTags.Remove(toDelete.Pop());
            }
        }

        public void CreateNotYetExisting(ToC toc)
        {
            _log?.Info("Scanning not yet existing entries...");
            int count = 0;
            foreach (var file in toc.Files)
            {
                if (!_loadedTags.ContainsKey(file))
                {
                    _loadedTags.Add(file, Array.Empty<string>());
                    ++count;
                }
            }
            _log?.Info("Created {0} new tag entries. Please fill them.", count);
        }

        public ISet<string> GetTagsForFile(string file)
        {
            if (_loadedTags.ContainsKey(file))
            {
                HashSet<string> tags = new HashSet<string>(_loadedTags[file].Select(x => x.ToTitleCase(_culture)));
                return tags;
            }
            return new HashSet<string>();
        }

        public ISet<string> GetTagsForFiles(IEnumerable<string> files)
        {
            HashSet<string> tags = new HashSet<string>();
            foreach (var file in files)
            {
                if (_loadedTags.ContainsKey(file))
                {
                    var fileTags = _loadedTags[file].Select(x => x.ToTitleCase(_culture));
                    foreach (var tag in fileTags)
                    {
                        tags.Add(tag);
                    }
                }
            }
            return tags;
        }
    }
}
