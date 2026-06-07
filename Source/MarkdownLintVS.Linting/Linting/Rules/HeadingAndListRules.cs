using System.Text.RegularExpressions;

using Markdig.Syntax;

namespace MarkdownLintVS.Linting.Rules
{
    /// <summary>
    /// MD001: Heading levels should only increment by one level at a time.
    /// </summary>
    public class MD001_HeadingIncrement : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD001;
        public override RuleInfo Info => _info;

        private const string _defaultTitlePattern = @"^\s*title\s*[:=]";

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            // Filter out headings in front matter (Markdig parses --- as setext heading markers)
            var headings = analysis.GetHeadings()
                .Where(h => !analysis.IsLineInFrontMatter(h.Line))
                .OrderBy(h => h.Line)
                .ToList();

            // Get front_matter_title pattern - empty string disables the feature
            var frontMatterTitlePattern = configuration.GetStringParameter("front_matter_title", _defaultTitlePattern);

            // Determine starting level based on front matter title
            var previousLevel = 0;
            if (!string.IsNullOrEmpty(frontMatterTitlePattern) && analysis.HasFrontMatterTitle(frontMatterTitlePattern))
            {
                // Front matter title acts as H1
                previousLevel = 1;
            }

            foreach (HeadingBlock heading in headings)
            {
                if (previousLevel > 0 && heading.Level > previousLevel + 1)
                {
                    var line = analysis.GetLine(heading.Line);
                    yield return CreateLineViolation(
                        heading.Line,
                        line,
                        $"Heading level should increment by one level at a time (expected h{previousLevel + 1}, found h{heading.Level})",
                        severity);
                }
                previousLevel = heading.Level;
            }
        }
    }

    /// <summary>
    /// MD003: Heading style should be consistent.
    /// </summary>
    public class MD003_HeadingStyle : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD003;
        public override RuleInfo Info => _info;

        private static readonly Regex _atxClosedPattern = new(@"^#{1,6}\s+.+\s+#{1,6}\s*$", RegexOptions.Compiled);

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            var style = configuration.GetStringParameter("style", "consistent");
            if (style == "false")
                yield break;

            string detectedStyle = null;

            foreach (HeadingBlock heading in analysis.GetHeadings())
            {
                if (analysis.IsLineInFrontMatter(heading.Line))
                    continue;

                var line = analysis.GetLine(heading.Line);
                var currentStyle = GetHeadingStyle(line, heading);

                if (style == "consistent")
                {
                    if (detectedStyle == null)
                    {
                        detectedStyle = currentStyle;
                    }
                    else if (currentStyle != detectedStyle)
                    {
                        // Handle setext_with_atx style - allow ATX for H3+
                        if (detectedStyle == "setext" && currentStyle == "atx" && heading.Level > 2)
                            continue;

                        yield return CreateLineViolation(
                            heading.Line,
                            line,
                            $"Heading style should be consistent (expected {detectedStyle}, found {currentStyle})",
                            severity);
                    }
                }
                else if (style == "setext_with_atx")
                {
                    // Setext for H1/H2, ATX for H3+
                    if (heading.Level <= 2)
                    {
                        if (currentStyle != "setext")
                        {
                            yield return CreateLineViolation(
                                heading.Line,
                                line,
                                $"Heading style should be setext for h1/h2 (found {currentStyle})",
                                severity);
                        }
                    }
                    else
                    {
                        if (currentStyle != "atx")
                        {
                            yield return CreateLineViolation(
                                heading.Line,
                                line,
                                $"Heading style should be atx for h3+ (found {currentStyle})",
                                severity);
                        }
                    }
                }
                else if (style == "setext_with_atx_closed")
                {
                    // Setext for H1/H2, ATX closed for H3+
                    if (heading.Level <= 2)
                    {
                        if (currentStyle != "setext")
                        {
                            yield return CreateLineViolation(
                                heading.Line,
                                line,
                                $"Heading style should be setext for h1/h2 (found {currentStyle})",
                                severity);
                        }
                    }
                    else
                    {
                        if (currentStyle != "atx_closed")
                        {
                            yield return CreateLineViolation(
                                heading.Line,
                                line,
                                $"Heading style should be atx_closed for h3+ (found {currentStyle})",
                                severity);
                        }
                    }
                }
                else if (currentStyle != style)
                {
                    yield return CreateLineViolation(
                        heading.Line,
                        line,
                        $"Heading style should be {style} (found {currentStyle})",
                        severity);
                }
            }
        }

        private string GetHeadingStyle(string line, HeadingBlock heading)
        {
            if (heading.IsSetext)
                return "setext";

            if (_atxClosedPattern.IsMatch(line))
                return "atx_closed";

            return "atx";
        }
    }

    /// <summary>
    /// MD004: Unordered list style should be consistent.
    /// </summary>
    public class MD004_UlStyle : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD004;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            var style = configuration.GetStringParameter("style", "consistent");
            if (style == "false")
                yield break;

            char? detectedMarker = null;
            var lastLevelMarker = new Dictionary<int, char>();

            foreach (ListBlock list in analysis.GetLists().Where(l => l.BulletType != '1'))
            {
                foreach (Block item in list)
                {
                    if (item is ListItemBlock listItem)
                    {
                        var line = analysis.GetLine(listItem.Line);
                        var marker = GetListMarker(line);
                        if (marker == null) continue;

                        var indent = GetIndentLevel(line);

                        if (style == "consistent")
                        {
                            if (detectedMarker == null)
                            {
                                detectedMarker = marker;
                            }
                            else if (marker != detectedMarker)
                            {
                                yield return CreateLineViolation(
                                    listItem.Line,
                                    line,
                                    $"Unordered list style should be consistent (expected '{GetMarkerName(detectedMarker.Value)}', found '{GetMarkerName(marker.Value)}')",
                                    severity);
                            }
                        }
                        else if (style == "sublist")
                        {
                            if (lastLevelMarker.TryGetValue(indent, out var expectedMarker))
                            {
                                if (marker != expectedMarker)
                                {
                                    yield return CreateLineViolation(
                                        listItem.Line,
                                        line,
                                        $"Unordered list style should be consistent within same level",
                                        severity);
                                }
                            }
                            lastLevelMarker[indent] = marker.Value;
                        }
                        else
                        {
                            var expectedMarker = GetExpectedMarker(style);
                            if (expectedMarker.HasValue && marker != expectedMarker)
                            {
                                yield return CreateLineViolation(
                                    listItem.Line,
                                    line,
                                    $"Unordered list style should use {GetMarkerName(expectedMarker.Value)}",
                                    severity);
                            }
                        }
                    }
                }
            }
        }

        private char? GetListMarker(string line)
        {
            var trimmed = line.TrimStart();
            if (trimmed.Length > 0 && (trimmed[0] == '*' || trimmed[0] == '-' || trimmed[0] == '+'))
                return trimmed[0];
            return null;
        }

        private int GetIndentLevel(string line)
        {
            var indent = 0;
            foreach (var c in line)
            {
                if (c == ' ') indent++;
                else if (c == '\t') indent += 4;
                else break;
            }
            return indent;
        }

        private char? GetExpectedMarker(string style)
        {
            return style switch
            {
                "asterisk" => '*',
                "plus" => '+',
                "dash" => '-',
                _ => null,
            };
        }

        private string GetMarkerName(char marker)
        {
            return marker switch
            {
                '*' => "asterisk",
                '+' => "plus",
                '-' => "dash",
                _ => marker.ToString(),
            };
        }
    }

    /// <summary>
    /// MD005: Inconsistent indentation for list items at the same level.
    /// </summary>
    public class MD005_ListIndent : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD005;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            var levelIndents = new Dictionary<int, int>();

            foreach (ListBlock list in analysis.GetLists())
            {
                // Only analyze top-level lists (nested lists are handled recursively)
                if (list.Parent is ListItemBlock)
                    continue;

                levelIndents.Clear();
                foreach (LintViolation violation in AnalyzeList(list, analysis, severity, levelIndents, 0))
                {
                    yield return violation;
                }
            }
        }

        private IEnumerable<LintViolation> AnalyzeList(ListBlock list, MarkdownDocumentAnalysis analysis,
            DiagnosticSeverity severity, Dictionary<int, int> levelIndents, int level)
        {
            // For ordered lists, we need to detect if they use right-alignment
            var isOrderedList = list.IsOrdered;
            var levelMarkerEndPositions = new Dictionary<int, int>();
            var isRightAligned = new Dictionary<int, bool?>();

            foreach (Block item in list)
            {
                if (item is ListItemBlock listItem)
                {
                    var line = analysis.GetLine(listItem.Line);
                    var indent = GetIndentLevel(line);
                    var markerEndPos = isOrderedList ? GetOrderedListMarkerEndPosition(line) : -1;

                    if (levelIndents.TryGetValue(level, out var expectedIndent))
                    {
                        if (indent != expectedIndent)
                        {
                            // For ordered lists, check if this could be right-aligned
                            if (isOrderedList && markerEndPos >= 0)
                            {
                                // Check if we've determined alignment for this level
                                if (!isRightAligned.TryGetValue(level, out var rightAligned))
                                {
                                    // First mismatch - determine if it's right-aligned
                                    if (levelMarkerEndPositions.TryGetValue(level, out var expectedMarkerEnd))
                                    {
                                        rightAligned = markerEndPos == expectedMarkerEnd;
                                        isRightAligned[level] = rightAligned;
                                    }
                                }

                                // If right-aligned and marker ends align, skip violation
                                if (rightAligned == true &&
                                    levelMarkerEndPositions.TryGetValue(level, out var expectedEnd) &&
                                    markerEndPos == expectedEnd)
                                {
                                    goto CheckNested;
                                }
                            }

                            yield return CreateLineViolation(
                                listItem.Line,
                                line,
                                $"Inconsistent indentation for list items at the same level (expected {expectedIndent}, found {indent})",
                                severity);
                        }
                    }
                    else
                    {
                        levelIndents[level] = indent;
                        if (isOrderedList && markerEndPos >= 0)
                        {
                            levelMarkerEndPositions[level] = markerEndPos;
                        }
                    }

                CheckNested:
                    // Check nested lists
                    foreach (Block child in listItem)
                    {
                        if (child is ListBlock nestedList)
                        {
                            foreach (LintViolation violation in AnalyzeList(nestedList, analysis, severity, levelIndents, level + 1))
                            {
                                yield return violation;
                            }
                        }
                    }
                }
            }
        }

        private int GetIndentLevel(string line)
        {
            var indent = 0;
            foreach (var c in line)
            {
                if (c == ' ') indent++;
                else if (c == '\t') indent += 4;
                else break;
            }
            return indent;
        }

        /// <summary>
        /// Gets the position where the ordered list marker ends (after the dot and space).
        /// Returns -1 if not an ordered list item.
        /// </summary>
        private int GetOrderedListMarkerEndPosition(string line)
        {
            var trimmedStart = 0;
            while (trimmedStart < line.Length && (line[trimmedStart] == ' ' || line[trimmedStart] == '\t'))
            {
                trimmedStart++;
            }

            // Find the number
            var numStart = trimmedStart;
            while (trimmedStart < line.Length && char.IsDigit(line[trimmedStart]))
            {
                trimmedStart++;
            }

            // Must have at least one digit
            if (trimmedStart == numStart)
                return -1;

            // Must be followed by . or )
            if (trimmedStart >= line.Length || (line[trimmedStart] != '.' && line[trimmedStart] != ')'))
                return -1;

            trimmedStart++; // Skip the . or )

            // The marker end position is right after the delimiter
            return trimmedStart;
        }
    }

    /// <summary>
    /// MD007: Unordered list indentation.
    /// </summary>
    public class MD007_UlIndent : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD007;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            // Determine indent value:
            // 1. Use explicit md_ul_indent parameter if set
            // 2. Fall back to EditorConfig indent_size if available
            // 3. Default to 2 spaces
            int indent;
            if (configuration.Parameters.ContainsKey("indent") || !string.IsNullOrEmpty(configuration.Value))
            {
                // Explicit md_ul_indent setting takes precedence
                indent = configuration.GetIntParameter("indent", 2);
            }
            else if (configuration.EditorConfigIndentSize.HasValue)
            {
                // Use EditorConfig indent_size as fallback
                indent = configuration.EditorConfigIndentSize.Value;
            }
            else
            {
                // Default to 2 spaces
                indent = 2;
            }

            var startIndented = configuration.GetBoolParameter("start_indented", false);
            var startIndent = configuration.GetIntParameter("start_indent", indent);

            // Only process top-level lists (not nested lists, which are handled recursively)
            foreach (ListBlock list in analysis.GetLists().Where(l => l.BulletType != '1' && l.Parent is MarkdownDocument))
            {
                foreach (LintViolation violation in AnalyzeList(list, analysis, severity, indent, startIndented, startIndent, 0))
                {
                    yield return violation;
                }
            }
        }

        private IEnumerable<LintViolation> AnalyzeList(ListBlock list, MarkdownDocumentAnalysis analysis,
            DiagnosticSeverity severity, int expectedIndent, bool startIndented, int startIndent, int level)
        {
            foreach (Block item in list)
            {
                if (item is ListItemBlock listItem)
                {
                    var line = analysis.GetLine(listItem.Line);
                    var actualIndent = GetIndentLevel(line);

                    // Calculate expected indentation for this level
                    int expected;
                    if (level == 0 && startIndented)
                    {
                        expected = startIndent;
                    }
                    else if (startIndented)
                    {
                        expected = startIndent + (level * expectedIndent);
                    }
                    else
                    {
                        expected = level * expectedIndent;
                    }

                    if (actualIndent != expected)
                    {
                        yield return CreateLineViolation(
                            listItem.Line,
                            line,
                            $"Unordered list indentation should be {expected} spaces (found {actualIndent})",
                            severity);
                    }

                    foreach (Block child in listItem)
                    {
                        if (child is ListBlock nestedList && nestedList.BulletType != '1')
                        {
                            foreach (LintViolation violation in AnalyzeList(nestedList, analysis, severity, expectedIndent, startIndented, startIndent, level + 1))
                            {
                                yield return violation;
                            }
                        }
                    }
                }
            }
        }

        private int GetIndentLevel(string line)
        {
            var indent = 0;
            foreach (var c in line)
            {
                if (c == ' ') indent++;
                else if (c == '\t') indent += 4;
                else break;
            }
            return indent;
        }
    }
}
