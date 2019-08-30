//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Template.ShortCodeImplementations;
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

        public ShortCodeParser(IList<ITemplateShortCode> shortCodes)
        {
            _shortCodes = new List<ITemplateShortCode>(shortCodes);
            _codeMatches = new Dictionary<string, Regex>(shortCodes.Count);
            BuildRegexes();
        }

        public void ConfigureDelegatedShortCodes(IList<DelegateShortCode> delegates)
        {
            _shortCodes.AddRange(delegates);
            foreach (var shortcode in delegates)
            {
                Regex match = new Regex($"(\\[{shortcode.Tag} .+\\])|(\\[{shortcode.Tag}\\])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                _codeMatches.Add(shortcode.Tag, match);
            }
        }

        private void BuildRegexes()
        {
            foreach (var shortcode in _shortCodes)
            {
                Regex match = new Regex($"(\\[{shortcode.Tag} .+\\])|(\\[{shortcode.Tag}\\])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                _codeMatches.Add(shortcode.Tag, match);
            }
        }

        public string Parse(string content)
        {
            StringBuilder result = new StringBuilder(content);
            foreach (var shortcode in _shortCodes)
            {
                var regex = _codeMatches[shortcode.Tag];
                var matches = regex.Matches(content);
                foreach (Match match in matches)
                {
                    var generated = shortcode.Generate(GetArguments(match.Value));
                    result.Replace(match.Value, generated);
                }
            }
            return result.ToString();
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
                        results.TryAdd(pair[0], Clean(pair[1]));
                    }
                }
            }

            return results;

        }

        private string Clean(string v)
        {
            return v.Substring(1, v.Length - 3);
        }
    }
}
