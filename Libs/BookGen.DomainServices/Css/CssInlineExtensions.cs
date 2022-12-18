//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace BookGen.DomainServices.Css
{
    internal static partial class CssInlineExtensions
    {
        [GeneratedRegex(@"\s{2,}", RegexOptions.None)]
        private static partial Regex NormalizeSpaceRegex();

        public static string NormalizeSpace(this string data)
        {
            return NormalizeSpaceRegex().Replace(data, @" ");
        }

        public static string NormalizeCharacter(this string data, char character)
        {
            var normalizeCharacterRegex = new Regex(character + "{2,}", RegexOptions.None);
            return normalizeCharacterRegex.Replace(data, character.ToString());
        }
    }
}
