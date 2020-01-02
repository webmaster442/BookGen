//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using BookGen.Core.Contracts.Configuration;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bookgen.Template.ShortCodeImplementations
{
    [Export(typeof(ITemplateShortCode))]
    public class Translate : ITemplateShortCode
    {
        private readonly IReadOnlyTranslations _translations;
        public static readonly Regex TranslateCheck = new Regex("^([A-Za-z_0-9]+)$", RegexOptions.Compiled);

        [ImportingConstructor]
        public Translate(IReadOnlyTranslations translations)
        {
            _translations = translations;
        }

        public string Tag => "\\?";

        public string Generate(IReadOnlyDictionary<string, string> arguments)
        {
            string argument = string.Empty;

            if (arguments.Count > 0)
                argument = arguments.Keys.First();

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
