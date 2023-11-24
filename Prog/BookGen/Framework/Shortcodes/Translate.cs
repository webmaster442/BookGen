//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel.Composition;

using BookGen.Interfaces.Configuration;

namespace BookGen.Framework.Shortcodes;

[Export(typeof(ITemplateShortCode))]
public sealed partial class Translate : ITemplateShortCode
{
    private readonly IReadOnlyTranslations _translations;

    [GeneratedRegex("^([A-Za-z_0-9]+)$", RegexOptions.Compiled, Constants.FiveSeconds)]
    private static partial Regex CreateTranslateRegex();

    [ImportingConstructor]
    public Translate(IReadOnlyTranslations translations)
    {
        _translations = translations;
    }

    public string Tag => "?";

    public bool CanCacheResult => false;

    public string Generate(IArguments arguments)
    {
        string argument = string.Empty;

        if (arguments.Count > 0)
            argument = arguments.First().Key;

        return DoTranslateForKey(_translations, argument);
    }

    public static string DoTranslateForKey(IReadOnlyTranslations translations, string key)
    {
        if (string.IsNullOrEmpty(key))
            return string.Empty;

        if (!CreateTranslateRegex().IsMatch(key))
            return $"Invalid tranlation key: {key}";
        else if (translations.ContainsKey(key))
            return translations[key];
        else
            return $"translation not found: '{key}'";
    }
}
