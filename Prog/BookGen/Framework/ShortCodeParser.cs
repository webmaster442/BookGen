//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;
using BookGen.Framework.Shortcodes;

namespace BookGen.Framework;

internal sealed partial class ShortCodeParser
{
    private readonly Dictionary<string, ITemplateShortCode> _shortCodesIndex;
    private readonly Dictionary<string, string> _codeResultCache;
    private readonly Translations _translations;
    private readonly ILog _log;

    private const string shortCodeStart = "<!--{";
    private const string shortCodeEnd = "}-->";

    [GeneratedRegex("(<!--\\{\\? [A-Za-z_0-9]+\\}-->)")]
    private partial Regex TranslateRegex();

    [GeneratedRegex(@"(<!--\{.+?\}-->)")]
    private partial Regex CodeRegex();

    public ShortCodeParser(IList<ITemplateShortCode> shortCodes,
                           Translations translations,
                           ILog log)
    {
        _shortCodesIndex = new Dictionary<string, ITemplateShortCode>(shortCodes.Count);
        _codeResultCache = new Dictionary<string, string>(100);
        _translations = translations;
        _log = log;
        AddShortcodesToLookupIndex(shortCodes);
    }

    public void AddShortcodeToLookupIndex(ITemplateShortCode shortCode)
    {
        if (!_shortCodesIndex.ContainsKey(shortCode.Tag))
        {
            _shortCodesIndex.Add(shortCode.Tag, shortCode);
        }
        else
        {
            _log.Warning("Shortcode has allready been registered: {0}. Duplicate entries cause unexpected behaviour.", shortCode.Tag);
        }
    }

    public void AddShortcodesToLookupIndex(IList<ITemplateShortCode> shortCodes)
    {
        foreach (ITemplateShortCode code in shortCodes)
        {
            AddShortcodeToLookupIndex(code);
        }
    }

    private static string GetTagKey(string value)
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
            return parts[0][shortCodeStart.Length..].Trim();
        }
    }

    private string AdditionalTranslate(string input)
    {
        MatchCollection matches = TranslateRegex().Matches(input);

        if (matches.Count == 0)
            return input;

        var cache = new StringBuilder(input);

        foreach (Match? match in matches)
        {
            if (match == null) continue;
            string? key = match.Value.Replace($"{shortCodeStart}? ", "").Replace(shortCodeEnd, "");

            string? text = Translate.DoTranslateForKey(_translations, key);

            cache.Replace(match.Value, text);
        }

        return cache.ToString();
    }

    private static ShortCodeArguments GetArguments(string value)
    {
        var results = new Dictionary<string, string>();

        string[]? firstpass = ShortCodeArgumentTokenizer.Split(value).Skip(1).ToArray();

        //no space means no additional arguments
        if (firstpass.Length < 1)
        {
            return new ShortCodeArguments();
        }
        else
        {
            foreach (string? token in firstpass)
            {
                string[]? pair = ShortCodeArgumentTokenizer.Split(token.Replace("=\"", " \"")).ToArray();
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
        return new ShortCodeArguments(results);
    }

    private static string RemoveStartingSpaceAndEndTags(string input)
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
        var result = new StringBuilder(content);
        MatchCollection matches = CodeRegex().Matches(content);
        foreach (Match? match in matches)
        {
            if (match != null)
            {
                string? tagKey = GetTagKey(match.Value);
                if (_codeResultCache.ContainsKey(match.Value))
                {
                    //Cache has it stored, so simply lookup and replace
                    result.Replace(match.Value, _codeResultCache[match.Value]);
                }
                else if (_shortCodesIndex.ContainsKey(tagKey))
                {
                    //It is a known shortcode, so run it
                    ITemplateShortCode? shortcode = _shortCodesIndex[tagKey];
                    string? generated = shortcode.Generate(GetArguments(match.Value));
                    result.Replace(match.Value, generated);
                    //For next iteration of it's occurance cache it if it's cacheable
                    if (shortcode.CanCacheResult)
                    {
                        _codeResultCache.Add(match.Value, generated);
                    }
                }
                else
                {
                    _log.Warning("Unknown shortcode: {0}", tagKey);
                }
            }
        }

        return AdditionalTranslate(result.ToString());
    }
}
