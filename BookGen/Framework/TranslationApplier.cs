//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using System.Text;
using System.Text.RegularExpressions;

namespace BookGen.Framework
{
    public static class TranslationApplier
    {
        private static readonly Regex translation = new Regex(@"(\{{[a-zA-Z0-9_]*\}})", RegexOptions.Compiled);

        public static string ApplyTranslations(string input, Translations translations)
        {
            StringBuilder result = new StringBuilder(input);
            var matches = translation.Matches(input);

            foreach (Match match in matches)
            {
                var key = match.Value.Replace("{{", "").Replace("}}", "");
                if (translations.ContainsKey(key))
                {
                    result.Replace(match.Value, translations[key]);
                }
                else
                {
                    result.Replace(match.Value, $"translation not found: {match.Value}");
                }
            }

            return result.ToString();
        }
    }
}
