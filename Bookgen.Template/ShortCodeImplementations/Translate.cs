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
        private readonly Regex _check;

        [ImportingConstructor]
        public Translate(Translations translations)
        {
            _translations = translations;
            _check = new Regex("[A-Za-z_0-9]+", RegexOptions.Compiled);
        }

        public string Tag => "\\?";

        public string Generate(IReadOnlyDictionary<string, string> arguments)
        {
            string argument = string.Empty;

            if (arguments.Count > 0)
                argument = arguments.Keys.First();

            if (!_check.IsMatch(argument))
                return $"Invalid tranlation key: {argument}";

            if (string.IsNullOrEmpty(argument))
                return string.Empty;
            else if (_translations.ContainsKey(argument))
                return _translations[argument];
            else
                return $"translation not found: '{argument}'";
        }
    }
}
