using System.Globalization;
using System.Text.RegularExpressions;

using Markdig.Syntax;

namespace MarkdownLintVS.Linting.Rules
{
    /// <summary>
    /// MD009: Trailing spaces.
    /// </summary>
    public class MD009_NoTrailingSpaces : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD009;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            var brSpaces = configuration.GetIntParameter("br_spaces", 2);
            var codeBlocks = configuration.GetBoolParameter("code_blocks", false);
            var listItemEmptyLines = configuration.GetBoolParameter("list_item_empty_lines", false);
            var strict = configuration.GetBoolParameter("strict", false);

            for (var i = 0; i < analysis.LineCount; i++)
            {
                if (analysis.IsLineInFrontMatter(i))
                    continue;

                // Skip code blocks unless code_blocks is true
                if (!codeBlocks && analysis.IsLineInCodeBlock(i))
                    continue;

                var line = analysis.GetLine(i);
                var trailingSpaces = CountTrailingSpaces(line);

                if (trailingSpaces > 0)
                {
                    // brSpaces must be >= 2 to take effect; value of 1 behaves like 0
                    var effectiveBrSpaces = brSpaces >= 2 ? brSpaces : 0;

                    // Allow exactly brSpaces for line breaks (unless strict mode)
                    if (!strict && effectiveBrSpaces > 0 && trailingSpaces == effectiveBrSpaces)
                        continue;

                    // Check for list item empty lines
                    if (listItemEmptyLines && string.IsNullOrWhiteSpace(line))
                        continue;

                    yield return CreateViolation(
                        i,
                        line.Length - trailingSpaces,
                        line.Length,
                        $"Trailing spaces ({trailingSpaces} found)",
                        severity,
                        "Remove trailing spaces");
                }
            }
        }

        private int CountTrailingSpaces(string line)
        {
            var count = 0;
            for (var i = line.Length - 1; i >= 0; i--)
            {
                if (line[i] == ' ')
                    count++;
                else
                    break;
            }
            return count;
        }
    }

    /// <summary>
    /// MD010: Hard tabs.
    /// </summary>
    public class MD010_NoHardTabs : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD010;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            var codeBlocks = configuration.GetBoolParameter("code_blocks", true);
            var ignoreCodeLanguagesStr = configuration.GetStringParameter("ignore_code_languages", "");
            var spacesPerTab = configuration.GetIntParameter("spaces_per_tab", 1);

            // Parse ignore_code_languages into a set for fast lookup
            var ignoreCodeLanguages = new HashSet<string>(
                ignoreCodeLanguagesStr.Split([',', ' '], System.StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim().ToLowerInvariant()));

            for (var i = 0; i < analysis.LineCount; i++)
            {
                if (analysis.IsLineInFrontMatter(i))
                    continue;

                if (analysis.IsLineInCodeBlock(i))
                {
                    // Skip if code_blocks is false
                    if (!codeBlocks)
                        continue;

                    // Check if this code block's language should be ignored
                    var language = analysis.GetCodeBlockLanguage(i);
                    if (language != null && ignoreCodeLanguages.Contains(language))
                        continue;
                }

                var line = analysis.GetLine(i);
                var tabIndex = line.IndexOf('\t');

                while (tabIndex >= 0)
                {
                    yield return CreateViolation(
                        i,
                        tabIndex,
                        tabIndex + 1,
                        "Hard tabs",
                        severity,
                        $"Replace tab with {spacesPerTab} spaces");

                    tabIndex = line.IndexOf('\t', tabIndex + 1);
                }
            }
        }
    }

    /// <summary>
    /// MD011: Reversed link syntax.
    /// </summary>
    public class MD011_NoReversedLinks : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD011;
        public override RuleInfo Info => _info;

        // Matches (url)[text] pattern where url looks like an actual URL
        private static readonly Regex _reversedLinkPattern = new(
            @"\(([^)]+)\)\[([^\]]+)\]",
            RegexOptions.Compiled);

        // R Markdown citation pattern: [@citekey] or [@key1; @key2] or [-@key]
        private static readonly Regex _citationPattern = new(
            @"^\[[-]?@[\w:-]+(?:;\s*[-]?@[\w:-]+)*\]$",
            RegexOptions.Compiled);

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            for (var i = 0; i < analysis.LineCount; i++)
            {
                if (analysis.IsLineInCodeBlock(i) || analysis.IsLineInFrontMatter(i))
                    continue;

                var line = analysis.GetLine(i);
                MatchCollection matches = _reversedLinkPattern.Matches(line);

                foreach (Match match in matches)
                {
                    var parenContent = match.Groups[1].Value;
                    var bracketContent = match.Groups[2].Value;

                    // Issue #1670: Skip R Markdown citation syntax (something)[@citekey]
                    // Citation patterns: [@key], [@key1; @key2], [-@key]
                    if (LooksLikeCitation(bracketContent))
                        continue;

                    // Only flag if the parentheses content looks like a URL
                    // This avoids false positives like (value)[0] or (obj)[key]
                    if (!LooksLikeUrl(parenContent))
                        continue;

                    yield return CreateViolation(
                        i,
                        match.Index,
                        match.Index + match.Length,
                        "Reversed link syntax",
                        severity,
                        "Swap link text and URL");
                }
            }
        }

        private static bool LooksLikeUrl(string text)
        {
            // Check for common URL patterns
            return text.Contains("://") ||
                   text.StartsWith("www.", StringComparison.OrdinalIgnoreCase) ||
                   text.StartsWith("/") ||
                   text.StartsWith("#") ||
                   text.StartsWith("./") ||
                   text.StartsWith("../") ||
                   text.StartsWith(".\\") ||
                   text.StartsWith("..\\");
        }

        private static bool LooksLikeCitation(string text)
        {
            // R Markdown/Pandoc citation syntax:
            // @citekey, -@citekey (suppress author), multiple keys separated by ;
            // Examples: @smith2020, -@jones2021, @key1; @key2; @key3
            if (string.IsNullOrEmpty(text))
                return false;

            // Check if text starts with @ or -@ (citation indicator)
            return text.StartsWith("@") || text.StartsWith("-@");
        }
    }

    /// <summary>
    /// MD012: Multiple consecutive blank lines.
    /// </summary>
    public class MD012_NoMultipleBlanks : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD012;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            var maximum = configuration.GetIntParameter("maximum", 1);
            var consecutiveBlanks = 0;

            for (var i = 0; i < analysis.LineCount; i++)
            {
                if (analysis.IsLineInCodeBlock(i) || analysis.IsLineInFrontMatter(i))
                {
                    consecutiveBlanks = 0;
                    continue;
                }

                if (analysis.IsBlankLine(i))
                {
                    consecutiveBlanks++;
                    if (consecutiveBlanks > maximum)
                    {
                        yield return CreateLineViolation(
                            i,
                            analysis.GetLine(i),
                            $"Multiple consecutive blank lines ({consecutiveBlanks} found, maximum {maximum} allowed)",
                            severity,
                            "Remove extra blank lines");
                    }
                }
                else
                {
                    consecutiveBlanks = 0;
                }
            }
        }
    }

    /// <summary>
    /// MD013: Line length.
    /// </summary>
    public class MD013_LineLength : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD013;
        public override RuleInfo Info => _info;

        private static readonly Regex _urlPattern = new(
            @"https?://[^\s\)]+",
            RegexOptions.Compiled);

        // Link/image reference definition: [label]: url "title"
        private static readonly Regex _linkRefDefinitionPattern = new(
            @"^\s*\[[^\]]+\]:\s*\S+",
            RegexOptions.Compiled);

        // Standalone link/image line (possibly with emphasis)
        private static readonly Regex _standaloneLinkPattern = new(
            @"^\s*(\*{0,2}|_{0,2})!?\[[^\]]*\]\([^\)]+\)(\*{0,2}|_{0,2})\s*$",
            RegexOptions.Compiled);

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            var lineLength = configuration.GetIntParameter("line_length", 80);
            var headingLineLength = configuration.GetIntParameter("heading_line_length", lineLength);
            var codeBlockLineLength = configuration.GetIntParameter("code_block_line_length", lineLength);
            var codeBlocks = configuration.GetBoolParameter("code_blocks", true);
            var tables = configuration.GetBoolParameter("tables", true);
            var headings = configuration.GetBoolParameter("headings", true);
            var strict = configuration.GetBoolParameter("strict", false);
            var stern = configuration.GetBoolParameter("stern", false);

            for (var i = 0; i < analysis.LineCount; i++)
            {
                if (analysis.IsLineInFrontMatter(i))
                    continue;

                var line = analysis.GetLine(i);

                // Always exempt link/image reference definitions and standalone link lines
                if (_linkRefDefinitionPattern.IsMatch(line) || _standaloneLinkPattern.IsMatch(line))
                    continue;

                var maxLength = lineLength;
                var isCodeBlock = analysis.IsLineInCodeBlock(i);
                var isHeading = line.TrimStart().StartsWith("#");

                if (isCodeBlock)
                {
                    if (!codeBlocks)
                        continue;
                    maxLength = codeBlockLineLength;
                }
                else if (isHeading)
                {
                    if (!headings)
                        continue;
                    maxLength = headingLineLength;
                }

                if (!tables && line.TrimStart().StartsWith("|"))
                    continue;

                // Use visual character count (grapheme clusters) instead of UTF-16 code units
                // This fixes Issue #1458: multi-byte Unicode characters counted incorrectly
                var visualLength = GetVisualLength(line);

                if (visualLength <= maxLength)
                    continue;

                // Check if line exceeds limit
                if (strict)
                {
                    // Strict mode: any line over limit is a violation
                    yield return CreateViolation(
                        i,
                        maxLength,
                        line.Length,
                        $"Line length is {visualLength} (maximum {maxLength})",
                        severity);
                }
                else
                {
                    // Check if there's whitespace beyond the limit (using visual position)
                    var hasWhitespaceBeyondLimit = HasWhitespaceBeyondVisualLimit(line, maxLength);

                    if (stern)
                    {
                        // Stern mode: report if there's whitespace beyond limit (fixable)
                        if (hasWhitespaceBeyondLimit)
                        {
                            yield return CreateViolation(
                                i,
                                maxLength,
                                line.Length,
                                $"Line length is {visualLength} (maximum {maxLength})",
                                severity);
                        }
                    }
                    else
                    {
                        // Default mode: only report if there's whitespace beyond limit
                        if (hasWhitespaceBeyondLimit)
                        {
                            yield return CreateViolation(
                                i,
                                maxLength,
                                line.Length,
                                $"Line length is {visualLength} (maximum {maxLength})",
                                severity);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the visual length of a string by counting grapheme clusters (text elements)
        /// instead of UTF-16 code units. This correctly handles:
        /// - Multi-byte Unicode characters (emoji, math symbols)
        /// - Combining characters (accents, diacritics)
        /// - Surrogate pairs
        /// </summary>
        private static int GetVisualLength(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            TextElementEnumerator enumerator = StringInfo.GetTextElementEnumerator(text);
            var count = 0;
            while (enumerator.MoveNext())
            {
                count++;
            }
            return count;
        }

        /// <summary>
        /// Checks if there's whitespace beyond the visual character limit.
        /// </summary>
        private static bool HasWhitespaceBeyondVisualLimit(string line, int maxLength)
        {
            TextElementEnumerator enumerator = StringInfo.GetTextElementEnumerator(line);
            var visualIndex = 0;

            while (enumerator.MoveNext())
            {
                if (visualIndex >= maxLength)
                {
                    var textElement = enumerator.GetTextElement();
                    if (textElement.Length > 0 && char.IsWhiteSpace(textElement[0]))
                    {
                        return true;
                    }
                }
                visualIndex++;
            }

            return false;
        }
    }

    /// <summary>
    /// MD014: Dollar signs used before commands without showing output.
    /// </summary>
    public class MD014_CommandsShowOutput : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD014;
        public override RuleInfo Info => _info;

        private static readonly Regex _dollarCommandPattern = new(
            @"^\s*\$\s+",
            RegexOptions.Compiled);

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            foreach (FencedCodeBlock codeBlock in analysis.GetFencedCodeBlocks())
            {
                var startLine = codeBlock.Line;
                var endLine = analysis.GetBlockEndLine(codeBlock);
                var hasOutput = false;
                var dollarLines = new List<int>();

                for (var i = startLine + 1; i < endLine; i++)
                {
                    var line = analysis.GetLine(i);
                    if (_dollarCommandPattern.IsMatch(line))
                    {
                        dollarLines.Add(i);
                    }
                    else if (!string.IsNullOrWhiteSpace(line))
                    {
                        hasOutput = true;
                    }
                }

                if (!hasOutput && dollarLines.Count > 0)
                {
                    foreach (var lineNum in dollarLines)
                    {
                        yield return CreateLineViolation(
                            lineNum,
                            analysis.GetLine(lineNum),
                            "Dollar signs used before commands without showing output",
                            severity,
                            "Remove dollar sign or add command output");
                    }
                }
            }
        }
    }
}
