//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Domain;
using BookGen.Interfaces;
using System.Globalization;
using System.Text;

namespace BookGen.DomainServices
{
    internal class TagUtils : ITagUtils
    {
        private readonly Dictionary<string, string[]> _loadedTags;
        private readonly ILog? _log;
        private readonly CultureInfo _culture;
        private readonly Dictionary<char, string> _symbolNames = new()
        {
            { '#', "sharp" },
            { '|', "pipe" },
            { '@', "at" },
            { '&', "ampersand" },
            { '~', "tide" },
            { '%', "percent" },
            { '+', "plus" },
            { '-', "minus" },
            { ':', "colon" },
            { ';', "semicolon" },
            { ',', "comma" },
            { '`', "backtick" },
            { '\'', "apoostrophe" },
            { '"', "quotationmark" },
            { '$', "dollar" },
            { '.', "dot" },
            { '!', "exclamation" },
            { '*', "star" },
            { '/', "slash" },
        };

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
            foreach (string? file in _loadedTags.Keys)
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
            foreach (string? file in toc.Files)
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
                var tags = new HashSet<string>(_loadedTags[file].Select(x => x.ToTitleCase(_culture)));
                return tags;
            }
            return new HashSet<string>();
        }

        public ISet<string> GetTagsForFiles(IEnumerable<string> files)
        {
            var tags = new HashSet<string>();
            foreach (string? file in files)
            {
                if (_loadedTags.ContainsKey(file))
                {
                    IEnumerable<string>? fileTags = _loadedTags[file].Select(x => x.ToTitleCase(_culture));
                    foreach (string? tag in fileTags)
                    {
                        tags.Add(tag);
                    }
                }
            }
            return tags;
        }

        public string GetUrlNiceName(string tag)
        {
            string ascii = AsciiEncodeLowerCase(tag);
            string replaced = ReplaceNotAllowedChars(ascii);
            if (string.IsNullOrEmpty(replaced))
                return "n-a";
            else
                return replaced;
        }

        private string ReplaceNotAllowedChars(string input)
        {
            var allowed = new HashSet<char>("abcdefghijklmnopqrstuvwxyz0123456789-_");
            var final = new StringBuilder(input.Length);
            foreach (char c in input)
            {
                if (allowed.Contains(c))
                    final.Append(c);
                else if (_symbolNames.ContainsKey(c))
                    final.Append(_symbolNames[c]);
                else
                    final.Append("-");
            }
            return final.ToString().Trim();
        }

        private static string AsciiEncodeLowerCase(string text)
        {
            text = text.Replace("?", "question");
            var ascii = new ASCIIEncoding();
            byte[] byteArray = Encoding.UTF8.GetBytes(text.Normalize(NormalizationForm.FormD));
            byte[] asciiArray = Encoding.Convert(Encoding.UTF8, Encoding.ASCII, byteArray);
            return ascii.GetString(asciiArray).Replace("?", "").ToLower();
        }
    }
}
