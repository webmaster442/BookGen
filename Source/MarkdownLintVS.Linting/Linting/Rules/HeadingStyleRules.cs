using System.Text.RegularExpressions;

using Markdig.Syntax;

namespace MarkdownLintVS.Linting.Rules
{
    /// <summary>
    /// MD018: No space after hash on atx style heading.
    /// </summary>
    public class MD018_NoMissingSpaceAtx : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD018;
        public override RuleInfo Info => _info;

        private static readonly Regex AtxNoSpacePattern = new(
            @"^#{1,6}[^#\s]",
            RegexOptions.Compiled);

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            foreach ((var lineNumber, var line) in analysis.GetAnalyzableLines())
            {
                if (AtxNoSpacePattern.IsMatch(line))
                {
                    yield return CreateLineViolation(
                        lineNumber,
                        line,
                        "No space after hash on atx style heading",
                        severity,
                        "Add space after hash");
                }
            }
        }
    }

    /// <summary>
    /// MD019: Multiple spaces after hash on atx style heading.
    /// </summary>
    public class MD019_NoMultipleSpaceAtx : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD019;
        public override RuleInfo Info => _info;

        private static readonly Regex AtxMultipleSpacePattern = new(
            @"^#{1,6}\s{2,}",
            RegexOptions.Compiled);

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            foreach (HeadingBlock heading in analysis.GetHeadings())
            {
                if (heading.IsSetext)
                    continue;

                var line = analysis.GetLine(heading.Line);
                if (AtxMultipleSpacePattern.IsMatch(line))
                {
                    yield return CreateLineViolation(
                        heading.Line,
                        line,
                        "Multiple spaces after hash on atx style heading",
                        severity,
                        "Use single space after hash");
                }
            }
        }
    }

    /// <summary>
    /// MD020: No space inside hashes on closed atx style heading.
    /// </summary>
    public class MD020_NoMissingSpaceClosedAtx : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD020;
        public override RuleInfo Info => _info;

        // Matches closed ATX headings: # Heading # or ## Heading ##
        // Using non-greedy .+? to capture content between opening and closing hashes
        private static readonly Regex ClosedAtxPattern = new(
            @"^(#{1,6})(.+?)(#{1,6})\s*$",
            RegexOptions.Compiled);

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            foreach ((var lineNumber, var line) in analysis.GetAnalyzableLines())
            {
                Match match = ClosedAtxPattern.Match(line);
                if (match.Success && match.Groups[1].Length == match.Groups[3].Length)
                {
                    var content = match.Groups[2].Value;
                    var hasMissingSpace = false;

                    // Check for missing space after opening hashes
                    if (content.Length > 0 && content[0] != ' ')
                    {
                        hasMissingSpace = true;
                    }

                    // Check for missing space before closing hashes
                    if (content.Length > 0 && content[content.Length - 1] != ' ')
                    {
                        hasMissingSpace = true;
                    }

                    if (hasMissingSpace)
                    {
                        yield return CreateLineViolation(
                            lineNumber,
                            line,
                            "No space inside hashes on closed atx style heading",
                            severity,
                            "Add space inside hashes");
                    }
                }
            }
        }
    }

    /// <summary>
    /// MD021: Multiple spaces inside hashes on closed atx style heading.
    /// </summary>
    public class MD021_NoMultipleSpaceClosedAtx : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD021;
        public override RuleInfo Info => _info;

        // Matches closed ATX headings with groups for leading hashes, content, and trailing hashes
        private static readonly Regex ClosedAtxPattern = new(
            @"^(#{1,6})(.+?)(#{1,6})\s*$",
            RegexOptions.Compiled);

        private static readonly Regex ClosedAtxMultipleSpacePattern = new(
            @"^#{1,6}\s{2,}.+|.+\s{2,}#{1,6}\s*$",
            RegexOptions.Compiled);

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            foreach ((var lineNumber, var line) in analysis.GetAnalyzableLines())
            {
                Match match = ClosedAtxPattern.Match(line);
                if (match.Success && match.Groups[1].Length == match.Groups[3].Length)
                {
                    if (ClosedAtxMultipleSpacePattern.IsMatch(line))
                    {
                        yield return CreateLineViolation(
                            lineNumber,
                            line,
                            "Multiple spaces inside hashes on closed atx style heading",
                            severity,
                            "Use single space inside hashes");
                    }
                }
            }
        }
    }

    /// <summary>
    /// MD022: Headings should be surrounded by blank lines.
    /// </summary>
    public class MD022_BlanksAroundHeadings : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD022;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            var linesAbove = configuration.GetIntParameter("lines_above", 1);
            var linesBelow = configuration.GetIntParameter("lines_below", 1);

            var firstContentLine = 0;
            while (firstContentLine < analysis.LineCount && analysis.IsLineInFrontMatter(firstContentLine))
            {
                firstContentLine++;
            }
            while (firstContentLine < analysis.LineCount && analysis.IsBlankLine(firstContentLine))
            {
                firstContentLine++;
            }

            foreach (HeadingBlock heading in analysis.GetHeadings())
            {
                if (analysis.IsLineInFrontMatter(heading.Line))
                    continue;

                // For setext headings, Markdig reports Line as the underline
                // The actual heading content starts one line before
                var startLine = heading.IsSetext ? heading.Line - 1 : heading.Line;
                var endLine = heading.IsSetext ? heading.Line : heading.Line;

                // Check lines above (except for first heading or just after front matter)
                if (startLine > 0 && startLine > firstContentLine)
                {
                    var blankAbove = 0;
                    for (var i = startLine - 1; i >= 0 && analysis.IsBlankLine(i); i--)
                    {
                        blankAbove++;
                    }

                    if (blankAbove < linesAbove)
                    {
                        yield return CreateLineViolation(
                            startLine,
                            analysis.GetLine(startLine),
                            $"Heading should be preceded by {linesAbove} blank line(s)",
                            severity,
                            "Add blank line before heading");
                    }
                }

                // Check lines below
                if (endLine < analysis.LineCount - 1)
                {
                    var blankBelow = 0;
                    for (var i = endLine + 1; i < analysis.LineCount && analysis.IsBlankLine(i); i++)
                    {
                        blankBelow++;
                    }

                    if (blankBelow < linesBelow)
                    {
                        yield return CreateLineViolation(
                            startLine,
                            analysis.GetLine(startLine),
                            $"Heading should be followed by {linesBelow} blank line(s)",
                            severity,
                            "Add blank line after heading");
                    }
                }
            }
        }
    }

    /// <summary>
    /// MD023: Headings must start at the beginning of the line.
    /// </summary>
    public class MD023_HeadingStartLeft : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD023;
        public override RuleInfo Info => _info;

        private static readonly Regex IndentedHeadingPattern = new(
            @"^\s+#{1,6}\s",
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
                if (IndentedHeadingPattern.IsMatch(line))
                {
                    yield return CreateLineViolation(
                        i,
                        line,
                        "Headings must start at the beginning of the line",
                        severity,
                        "Remove leading whitespace");
                }
            }
        }
    }

    /// <summary>
    /// MD024: Multiple headings with the same content.
    /// </summary>
    public class MD024_NoDuplicateHeading : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD024;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            var mode = configuration.GetStringParameter("style", "siblings_only");

            var headings = analysis.GetHeadings().ToList();
            var seenHeadings = new Dictionary<string, (int Line, int Level)>();
            var levelStack = new Stack<(int Level, int Line)>();

            foreach (HeadingBlock heading in headings)
            {
                var content = GetHeadingContent(heading, analysis);

                // Track nesting: pop back to find the parent context for this heading
                while (levelStack.Count > 0 && levelStack.Peek().Level >= heading.Level)
                {
                    levelStack.Pop();
                }

                var key = mode == "all"
                    ? content
                    : $"{string.Join("-", levelStack.Select(s => $"{s.Level}L{s.Line}"))}:{content}";

                levelStack.Push((heading.Level, heading.Line));

                if (seenHeadings.TryGetValue(key, out (int Line, int Level) existing))
                {
                    yield return CreateLineViolation(
                        heading.Line,
                        analysis.GetLine(heading.Line),
                        $"Multiple headings with the same content: \"{content}\"",
                        severity);
                }
                else
                {
                    seenHeadings[key] = (heading.Line, heading.Level);
                }
            }
        }

        private string GetHeadingContent(HeadingBlock heading, MarkdownDocumentAnalysis analysis)
        {
            var line = analysis.GetLine(heading.Line);
            // Remove heading markers
            var content = line.TrimStart('#', ' ').TrimEnd('#', ' ');
            return content.ToLowerInvariant();
        }
    }

    /// <summary>
    /// MD025: Multiple top-level headings in the same document.
    /// </summary>
    public class MD025_SingleTitle : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD025;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            var level = configuration.GetIntParameter("level", 1);
            var frontMatterTitle = configuration.GetStringParameter("front_matter_title", "title");

            var h1Headings = analysis.GetHeadings()
                .Where(h => h.Level == level)
                .ToList();

            // Skip first one, flag the rest
            for (var i = 1; i < h1Headings.Count; i++)
            {
                yield return CreateLineViolation(
                    h1Headings[i].Line,
                    analysis.GetLine(h1Headings[i].Line),
                    $"Multiple top-level headings in the same document",
                    severity);
            }
        }
    }

    /// <summary>
    /// MD026: Trailing punctuation in heading.
    /// </summary>
    public class MD026_NoTrailingPunctuation : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD026;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            var punctuation = configuration.GetStringParameter("punctuation", ".,;:!。，；：！");

            foreach (HeadingBlock heading in analysis.GetHeadings())
            {
                var line = analysis.GetLine(heading.Line);
                var content = line.TrimEnd();
                
                // Remove closing hashes for closed atx
                while (content.EndsWith("#"))
                    content = content.TrimEnd('#').TrimEnd();

                if (content.Length > 0 && punctuation.Contains(content[content.Length - 1]))
                {
                    yield return CreateLineViolation(
                        heading.Line,
                        line,
                        $"Trailing punctuation in heading: '{content[content.Length - 1]}'",
                        severity,
                        "Remove trailing punctuation");
                }
            }
        }
    }
}
