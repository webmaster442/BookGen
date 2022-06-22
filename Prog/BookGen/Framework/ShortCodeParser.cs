﻿//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Domain;
using BookGen.Framework.Scripts;
using BookGen.Framework.Shortcodes;

namespace BookGen.Framework
{
    internal class ShortCodeParser
    {
        private readonly Dictionary<string, ITemplateShortCode> _shortCodesIndex;
        private readonly Dictionary<string, string> _codeResultCache;
        private readonly Translations _translations;
        private readonly CsharpScriptHandler _scriptHandler;
        private readonly ILog _log;

        private const string shortCodeStart = "<!--{";
        private const string shortCodeEnd = "}-->";

        private readonly Regex TranslateRegex = new Regex("(<!--\\{\\? [A-Za-z_0-9]+\\}-->)", RegexOptions.Compiled);
        private readonly Regex CodeRegex = new Regex(@"(<!--\{.+?\}-->)", RegexOptions.Compiled);

        public ShortCodeParser(IList<ITemplateShortCode> shortCodes,
                               CsharpScriptHandler scriptHandler,
                               Translations translations,
                               ILog log)
        {
            _shortCodesIndex = new Dictionary<string, ITemplateShortCode>(shortCodes.Count);
            _codeResultCache = new Dictionary<string, string>(100);
            _scriptHandler = scriptHandler;
            _translations = translations;
            _log = log;
            AddShortcodesToLookupIndex(shortCodes);
        }

        public void AddShortcodesToLookupIndex(IList<ITemplateShortCode> shortCodes)
        {
            foreach (var code in shortCodes)
            {
                if (!_shortCodesIndex.ContainsKey(code.Tag))
                {
                    _shortCodesIndex.Add(code.Tag, code);
                }
                else
                {
                    _log.Warning("Shortcode has allready been registered: {0}. Duplicate entries cause unexpected behaviour.", code.Tag);
                }
            }
        }

        private string GetTagKey(string value)
        {
            string[] parts = value.Split(' ');
            if (parts.Length == 1)
            {
                //value is eg. <!--{foo}-->
                int len = value.Length - shortCodeStart.Length - shortCodeEnd.Length;
                return value.Substring(shortCodeStart.Length, len);
            }
            else
            {
                return parts[0].Substring(shortCodeStart.Length).Trim();
            }
        }

        private string AdditionalTranslate(string input)
        {
            MatchCollection matches = TranslateRegex.Matches(input);

            if (matches.Count == 0)
                return input;

            StringBuilder cache = new StringBuilder(input);

            foreach (Match? match in matches)
            {
                if (match == null) continue;
                var key = match.Value.Replace($"{shortCodeStart}? ", "").Replace(shortCodeEnd, "");

                var text = Translate.DoTranslateForKey(_translations, key);

                cache.Replace(match.Value, text);
            }

            return cache.ToString();
        }

        private Arguments GetArguments(string value)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();

            var firstpass = ShortCodeArgumentTokenizer.Split(value).Skip(1).ToArray();

            //no space means no additional arguments
            if (firstpass.Length < 1)
            {
                return new Arguments();
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
                        results.TryAdd(pair[0].Replace(shortCodeEnd, ""), string.Empty);
                    }
                }
            }
            return new Arguments(results);
        }

        private string RemoveStartingSpaceAndEndTags(string input)
        {
            //input string will be in following format: "Assets/bootstrap.min.css"}-->
            if (input.StartsWith("\"") && input.EndsWith("\"}-->"))
            {
                //need to retgurn only: Assets/bootstrap.min.css
                return input.Substring(1, input.Length - (shortCodeEnd.Length + 2));
            }
            return input;
        }

        public string Parse(string content)
        {
            StringBuilder result = new StringBuilder(content);
            MatchCollection matches = CodeRegex.Matches(content);
            foreach (Match? match in matches)
            {
                if (match != null)
                {
                    var tagKey = GetTagKey(match.Value);
                    if (_codeResultCache.ContainsKey(match.Value))
                    {
                        //Cache has it stored, so simply lookup and replace
                        result.Replace(match.Value, _codeResultCache[match.Value]);
                    }
                    else if (_shortCodesIndex.ContainsKey(tagKey))
                    {
                        //It is a known shortcode, so run it
                        var shortcode = _shortCodesIndex[tagKey];
                        var generated = shortcode.Generate(GetArguments(match.Value));
                        result.Replace(match.Value, generated);
                        //For next iteration of it's occurance cache it if it's cacheable
                        if (shortcode.CanCacheResult)
                        {
                            _codeResultCache.Add(match.Value, generated);
                        }
                    }
                    else if (_scriptHandler.IsKnownScript(tagKey))
                    {
                        var scriptResult = _scriptHandler.ExecuteScript(tagKey, GetArguments(match.Value));
                        result.Replace(match.Value, scriptResult);
                    }
                    else
                    {
                        _log.Warning("Unknown shortcode or script: {0}", tagKey);
                    }
                }
            }

            return AdditionalTranslate(result.ToString());
        }
    }
}
