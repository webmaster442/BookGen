//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace BookGen.DomainServices.Css
{
    internal static partial class CssToXPathRegexes
    {
        [GeneratedRegex(@"\[([^\]~\$\*\^\|\!]+)(=[^\]]+)?\]", RegexOptions.Multiline)]
        public static partial Regex Attributes();

        [GeneratedRegex(@"\s*,\s*", RegexOptions.Multiline)]
        public static partial Regex MultiQuery();

        [GeneratedRegex(@"\s*(\+|~|>)\s*", RegexOptions.Multiline)]
        public static partial Regex SpecialChars();

        [GeneratedRegex(@"([a-zA-Z0-9_\-\*])~([a-zA-Z0-9_\-\*])", RegexOptions.Multiline)]
        public static partial Regex Sibblings1();

        [GeneratedRegex(@"([a-zA-Z0-9_\-\*])\+([a-zA-Z0-9_\-\*])", RegexOptions.Multiline)]
        public static partial Regex Sibblings2();

        [GeneratedRegex(@"([a-zA-Z0-9_\-\*])>([a-zA-Z0-9_\-\*])", RegexOptions.Multiline)]
        public static partial Regex Sibblings3();

        [GeneratedRegex(@"\[([^=]+)=([^'|""][^\]]*)\]", RegexOptions.Multiline)]
        public static partial Regex Unescaped();

        [GeneratedRegex(@"(^|[^a-zA-Z0-9_\-\*])(#|\.)([a-zA-Z0-9_\-]+)", RegexOptions.Multiline)]
        public static partial Regex Self1();

        [GeneratedRegex(@"([\>\+\|\~\,\s])([a-zA-Z\*]+)", RegexOptions.Multiline)]
        public static partial Regex Self2();

        [GeneratedRegex(@"\s+\/\/", RegexOptions.Multiline)]
        public static partial Regex Self3();

        [GeneratedRegex(@"([a-zA-Z0-9_\-\*]+):first-child", RegexOptions.Multiline)]
        public static partial Regex FirstChild();

        [GeneratedRegex(@"([a-zA-Z0-9_\-\*]+):last-child", RegexOptions.Multiline)]
        public static partial Regex LastChild();

        [GeneratedRegex(@"([a-zA-Z0-9_\-\*]+):only-child", RegexOptions.Multiline)]
        public static partial Regex OnlyChild();

        [GeneratedRegex(@"([a-zA-Z0-9_\-\*]+):empty", RegexOptions.Multiline)]
        public static partial Regex Empty();

        [GeneratedRegex(@"\[([a-zA-Z0-9_\-]+)\|=([^\]]+)\]", RegexOptions.Multiline)]
        public static partial Regex AttribPipeEquals();

        [GeneratedRegex(@"\[([a-zA-Z0-9_\-]+)\*=([^\]]+)\]", RegexOptions.Multiline)]
        public static partial Regex AttribStarEquals();

        [GeneratedRegex(@"\[([a-zA-Z0-9_\-]+)~=([^\]]+)\]", RegexOptions.Multiline)]
        public static partial Regex AttribTildeEquals();

        [GeneratedRegex(@"\[([a-zA-Z0-9_\-]+)\^=([^\]]+)\]", RegexOptions.Multiline)]
        public static partial Regex AttribCaretEquals();

        [GeneratedRegex(@"\[([a-zA-Z0-9_\-]+)\!=([^\]]+)\]", RegexOptions.Multiline)]
        public static partial Regex AttribNotEquals();

        [GeneratedRegex(@"#([a-zA-Z0-9_\-]+)", RegexOptions.Multiline)]
        public static partial Regex Ids();

        [GeneratedRegex(@"\.([a-zA-Z0-9_\-]+)", RegexOptions.Multiline)]
        public static partial Regex Classes();

        [GeneratedRegex(@"\]\[([^\]]+)", RegexOptions.Multiline)]
        public static partial Regex NormalizeMultipleFilters();

    }
}
