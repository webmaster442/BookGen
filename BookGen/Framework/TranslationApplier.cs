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
        private static Regex translation = new Regex(@"(\{[a-zA-Z0-9_]*\})", RegexOptions.Compiled);

        public static void ApplyTranslations(StringBuilder input, Translations translations)
        {
            var matches = translation.Matches(input.ToString());
            foreach (Match match in matches)
            {
                var key = match.Value.Replace("{", "").Replace("}", "");
                if (translations.ContainsKey(key))
                {
                    input.Replace(match.Value, translations[key]);
                }
                else
                {
                    input.Replace(match.Value, $"translation not found: {match.Value}");
                }
            }
        }

        public static string ApplyTranslations(string input, Translations translations)
        {
            var builder = new StringBuilder(input);
            ApplyTranslations(builder, translations);
            return builder.ToString();
        }
    }
}
