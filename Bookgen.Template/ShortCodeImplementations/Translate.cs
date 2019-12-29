using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bookgen.Template.ShortCodeImplementations
{
    [Export(typeof(ITemplateShortCode))]
    public class Translate : ITemplateShortCode
    {
        private readonly Translations _translations;
        public static readonly Regex TranslateCheck = new Regex("^([A-Za-z_0-9]+)$", RegexOptions.Compiled);

        [ImportingConstructor]
        public Translate(Translations translations)
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

        public static string DoTranslateForKey(Translations translations, string key)
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
