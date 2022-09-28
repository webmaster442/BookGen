//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api.Configuration;
using BookGen.Interfaces;
using System.ComponentModel.Composition;

namespace BookGen.Framework.Shortcodes
{
    [Export(typeof(ITemplateShortCode))]
    public sealed class Translate : ITemplateShortCode
    {
        private readonly IReadOnlyTranslations _translations;
        public static readonly Regex TranslateCheck = new Regex("^([A-Za-z_0-9]+)$", RegexOptions.Compiled);

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

            if (!TranslateCheck.IsMatch(key))
                return $"Invalid tranlation key: {key}";
            else if (translations.ContainsKey(key))
                return translations[key];
            else
                return $"translation not found: '{key}'";
        }
    }
}
