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

        public Dictionary<string, string[]> TagCollection => _loadedTags;

        public TagUtils()
        {
            _loadedTags = new();
        }

        public TagUtils(Dictionary<string, string[]> loadedTags, ILog log)
        {
            _loadedTags = loadedTags;
            _log = log;
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

        public ISet<string> TagsForFile(string file)
        {
            if (_loadedTags.ContainsKey(file))
            {
                HashSet<string> tags = new HashSet<string>(_loadedTags[file].Select(x => x.ToTitleCase(CultureInfo.InvariantCulture)));
                return tags;
            }
            return new HashSet<string>();
        }

        public ISet<string> TagsForFiles(IEnumerable<string> files)
        {
            HashSet<string> tags = new HashSet<string>();
            foreach (var file in files)
            {
                if (_loadedTags.ContainsKey(file))
                {
                    var fileTags = _loadedTags[file].Select(x => x.ToTitleCase(CultureInfo.InvariantCulture));
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
