//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Template.ShortCodeImplementations;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BookGen.Framework
{
    internal class ShortCodeParser
    {
        private readonly List<ITemplateShortCode> _shortCodes;
        private readonly Dictionary<string, Regex> _codeMatches;
        private readonly Translations _translations;

        public ShortCodeParser(IList<ITemplateShortCode> shortCodes, Translations translations)
        {
            _shortCodes = new List<ITemplateShortCode>(shortCodes.Count);
            _codeMatches = new Dictionary<string, Regex>(shortCodes.Count);
            _translations = translations;
            ConfigureShortCodes(shortCodes);
        }

        public void ConfigureShortCodes(IList<ITemplateShortCode> codes)
        {
            _shortCodes.AddRange(codes);
            foreach (var shortcode in codes)
            {
                Regex match = new Regex($"(<<{shortcode.Tag} .+>>)|(<<{shortcode.Tag}>>)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                _codeMatches.Add(shortcode.Tag, match);
            }
        }

        public string Parse(string content)
        {
            StringBuilder result = new StringBuilder(content);
            foreach (var shortcode in _shortCodes)
            {
                Regex regex = _codeMatches[shortcode.Tag];
                MatchCollection matches = regex.Matches(content);
                foreach (Match? match in matches)
                {
                    if (match != null)
                    {
                        var generated = shortcode.Generate(GetArguments(match.Value));
                        result.Replace(match.Value, generated);
                    }
                }
            }
            return AdditionalTranslate(result.ToString());
        }

        private string AdditionalTranslate(string input)
        {
            MatchCollection matches = Translate.TranslateCheck.Matches(input);

            if (matches.Count == 0)
                return input;

            StringBuilder cache = new StringBuilder(input);

            foreach (Match? match in matches)
            {
                if (match == null) continue;
                var key = match.Value.Replace("<<? ", "").Replace(">>", "");

                var text = Translate.DoTranslateForKey(_translations, key);

                cache.Replace(match.Value, text);
            }

            return cache.ToString();
        }

        private Dictionary<string, string> GetArguments(string value)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();

            var firstpass = ShortCodeArgumentTokenizer.Split(value).Skip(1).ToArray();

            //no space means no additional arguments
            if (firstpass.Length < 1)
            {
                return results;
            }
            else
            {
                foreach (var token in firstpass)
                {
                    var pair = ShortCodeArgumentTokenizer.Split(token.Replace("=\"", " \"")).ToArray();
                    if (pair.Length == 2)
                    {
                        results.TryAdd(pair[0], RemoveStartingSpaceAndEndTags(pair[1]));
                    }
                    else
                    {
                        results.TryAdd(pair[0].Replace(">>", ""), string.Empty);
                    }
                }
            }
            return results;
        }

        private string RemoveStartingSpaceAndEndTags(string v)
        {
            //removes starting space and end >> tags
            return v.Substring(1, v.Length - 4);
        }
    }
}
