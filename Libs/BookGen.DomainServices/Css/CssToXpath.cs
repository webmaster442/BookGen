//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace BookGen.DomainServices.Css
{
    internal sealed class CssToXpath
    {
        private readonly Dictionary<Regex, string> _replaceTable;

        public CssToXpath()
        {
            _replaceTable = new Dictionary<Regex, string>
            {
                { CssToXPathRegexes.Attributes(), "[@$1$2]" },
                { CssToXPathRegexes.MultiQuery(), "|" },
                { CssToXPathRegexes.SpecialChars(), "$1" },
                { CssToXPathRegexes.Sibblings1(), "$1/following-sibling::$2" },
                { CssToXPathRegexes.Sibblings2(), "$1/following-sibling::*[1]/self::$2" },
                { CssToXPathRegexes.Sibblings3(), "$1/$2" },
                { CssToXPathRegexes.Unescaped(), "[$1='$2']" },
                { CssToXPathRegexes.Self1(), "$1*$2$3"  },
                { CssToXPathRegexes.Self2(), "$1//$2" },
                { CssToXPathRegexes.Self3(), "//"  },
                { CssToXPathRegexes.FirstChild(), "*[1]/self::$1" },
                { CssToXPathRegexes.LastChild(), "$1[not(following-sibling::*)]" },
                { CssToXPathRegexes.OnlyChild(),  "*[last()=1]/self::$1" },
                { CssToXPathRegexes.Empty(), "$1[not(*) and not(normalize-space())]" },
                { CssToXPathRegexes.AttribPipeEquals(), "[@$1=$2 or starts-with(@$1,concat($2,'-'))]" },
                { CssToXPathRegexes.AttribStarEquals(),  "[contains(@$1,$2)]" },
                { CssToXPathRegexes.AttribTildeEquals(), "[contains(concat(' ',normalize-space(@$1),' '),concat(' ',$2,' '))]" },
                { CssToXPathRegexes.AttribCaretEquals(),  "[starts-with(@$1,$2)]" },
                { CssToXPathRegexes.AttribNotEquals(),  "[not(@$1) or @$1!=$2]" },
                { CssToXPathRegexes.Ids(), "[@id='$1']" },
                { CssToXPathRegexes.Classes(), "[contains(concat(' ',normalize-space(@class),' '),' $1 ')]" },
                { CssToXPathRegexes.NormalizeMultipleFilters(),  " and ($1)" },
            };
        }

        public string Transform(string css)
        {
            string result = css;
            foreach (var replacement in _replaceTable)
            {
                result = replacement.Key.Replace(result, replacement.Value);
            }
            return $"//{result}";
        }
    }
}
