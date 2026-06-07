using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace MarkdownLintVS.Linting.Rules
{
    /// <summary>
    /// MD040: Fenced code blocks should have a language specified.
    /// </summary>
    public class MD040_FencedCodeLanguage : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD040;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            var allowedLanguages = configuration.GetStringParameter("allowed_languages", "")
                .Split(',')
                .Select(l => l.Trim().ToLowerInvariant())
                .Where(l => !string.IsNullOrEmpty(l))
                .ToHashSet();

            var languageOnly = configuration.GetBoolParameter("language_only", false);

            foreach (FencedCodeBlock codeBlock in analysis.GetFencedCodeBlocks())
            {
                var language = codeBlock.Info?.Trim();

                if (string.IsNullOrEmpty(language))
                {
                    yield return CreateLineViolation(
                        codeBlock.Line,
                        analysis.GetLine(codeBlock.Line),
                        "Fenced code blocks should have a language specified",
                        severity,
                        "Add language identifier");
                }
                else if (allowedLanguages.Count > 0 && !allowedLanguages.Contains(language.ToLowerInvariant()))
                {
                    yield return CreateLineViolation(
                        codeBlock.Line,
                        analysis.GetLine(codeBlock.Line),
                        $"Language '{language}' is not in the allowed list",
                        severity);
                }
            }
        }
    }

    /// <summary>
    /// MD041: First line in a file should be a top-level heading.
    /// </summary>
    public class MD041_FirstLineHeading : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD041;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            var level = configuration.GetIntParameter("level", 1);
            var frontMatterTitle = configuration.GetStringParameter("front_matter_title", "title");

            // Skip if file is empty
            var firstNonBlank = analysis.GetFirstNonBlankLine();
            if (firstNonBlank < 0)
                yield break;

            // Skip front matter
            var startLine = 0;
            if (analysis.Lines.Length > 0 && analysis.Lines[0].Trim() == "---")
            {
                for (var i = 1; i < analysis.Lines.Length; i++)
                {
                    if (analysis.Lines[i].Trim() == "---" || analysis.Lines[i].Trim() == "...")
                    {
                        // Check for front_matter_title
                        if (!string.IsNullOrEmpty(frontMatterTitle))
                        {
                            for (var j = 1; j < i; j++)
                            {
                                if (analysis.Lines[j].TrimStart().StartsWith(frontMatterTitle + ":"))
                                    yield break; // Has title in front matter
                            }
                        }
                        startLine = i + 1;
                        break;
                    }
                }
            }

            // Find first non-blank line after front matter
            while (startLine < analysis.LineCount && analysis.IsBlankLine(startLine))
            {
                startLine++;
            }

            if (startLine >= analysis.LineCount)
                yield break;

            // Check if the first non-blank content after front matter is a heading of the correct level.
            // Markdig's HeadingBlock.Line can be off by one around YAML front matter boundaries.
            HeadingBlock firstHeading = analysis.GetHeadings().FirstOrDefault(h => h.Line >= startLine - 1);

            if (firstHeading == null || (firstHeading.Line != startLine && firstHeading.Line != startLine - 1))
            {
                yield return CreateLineViolation(
                    startLine,
                    analysis.GetLine(startLine),
                    $"First line in a file should be a top-level heading (h{level})",
                    severity,
                    "Add heading at start of document");
            }
            else if (firstHeading.Level != level)
            {
                yield return CreateLineViolation(
                    startLine,
                    analysis.GetLine(startLine),
                    $"First heading should be level {level} (found h{firstHeading.Level})",
                    severity);
            }
        }
    }

    /// <summary>
    /// MD042: No empty links.
    /// </summary>
    public class MD042_NoEmptyLinks : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD042;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            foreach (LinkInline link in analysis.GetLinks())
            {
                var url = link.Url;
                var hasEmptyUrl = string.IsNullOrWhiteSpace(url) || url == "#";

                if (hasEmptyUrl)
                {
                    (var Line, var Column) = analysis.GetPositionFromOffset(link.Span.Start);
                    yield return CreateViolation(
                        Line,
                        Column,
                        Column + link.Span.Length,
                        "No empty links",
                        severity,
                        "Add link destination");
                }
            }
        }
    }

    /// <summary>
    /// MD045: Images should have alternate text (alt text).
    /// </summary>
    public class MD045_NoAltText : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD045;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            foreach (LinkInline link in analysis.GetLinks().Where(l => l.IsImage))
            {
                var altText = GetLinkText(link);

                if (string.IsNullOrWhiteSpace(altText))
                {
                    (var Line, var Column) = analysis.GetPositionFromOffset(link.Span.Start);
                    yield return CreateViolation(
                        Line,
                        Column,
                        Column + link.Span.Length,
                        "Images should have alternate text (alt text)",
                        severity,
                        "Add alt text to image");
                }
            }
        }

        private string GetLinkText(LinkInline link)
        {
            var text = "";
            foreach (Inline child in link)
            {
                if (child is LiteralInline literal)
                    text += literal.Content.ToString();
            }
            return text;
        }
    }

    /// <summary>
    /// MD046: Code block style.
    /// </summary>
    public class MD046_CodeBlockStyle : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD046;
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

            string detectedStyle = null;
            var allCodeBlocks = analysis.GetCodeBlocks().ToList();
            var fencedBlocks = analysis.GetFencedCodeBlocks().ToList();

            foreach (CodeBlock codeBlock in allCodeBlocks)
            {
                var isFenced = fencedBlocks.Any(f => f.Span.Start == codeBlock.Span.Start);
                var currentStyle = isFenced ? "fenced" : "indented";

                if (style == "consistent")
                {
                    if (detectedStyle == null)
                    {
                        detectedStyle = currentStyle;
                    }
                    else if (currentStyle != detectedStyle)
                    {
                        yield return CreateLineViolation(
                            codeBlock.Line,
                            analysis.GetLine(codeBlock.Line),
                            $"Code block style should be consistent (expected {detectedStyle})",
                            severity);
                    }
                }
                else if (currentStyle != style)
                {
                    yield return CreateLineViolation(
                        codeBlock.Line,
                        analysis.GetLine(codeBlock.Line),
                        $"Code block style should be {style}",
                        severity);
                }
            }
        }
    }

    /// <summary>
    /// MD047: Files should end with a single newline character.
    /// </summary>
    public class MD047_SingleTrailingNewline : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD047;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            if (analysis.Text.Length == 0)
                yield break;

            var lastLine = analysis.LineCount - 1;

            if (!analysis.EndsWithNewline())
            {
                yield return CreateLineViolation(
                    lastLine,
                    analysis.GetLine(lastLine),
                    "Files should end with a single newline character",
                    severity,
                    "Add newline at end of file");
            }
            else if (analysis.EndsWithMultipleNewlines())
            {
                yield return CreateLineViolation(
                    lastLine,
                    analysis.GetLine(lastLine),
                    "Files should end with a single newline character (multiple found)",
                    severity,
                    "Remove extra newlines at end of file");
            }
        }
    }

    /// <summary>
    /// MD048: Code fence style.
    /// </summary>
    public class MD048_CodeFenceStyle : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD048;
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

            string detectedStyle = null;

            foreach (FencedCodeBlock codeBlock in analysis.GetFencedCodeBlocks())
            {
                var line = analysis.GetLine(codeBlock.Line);
                var currentStyle = line.TrimStart().StartsWith("~") ? "tilde" : "backtick";

                if (style == "consistent")
                {
                    if (detectedStyle == null)
                    {
                        detectedStyle = currentStyle;
                    }
                    else if (currentStyle != detectedStyle)
                    {
                        yield return CreateLineViolation(
                            codeBlock.Line,
                            line,
                            $"Code fence style should be consistent (expected {detectedStyle})",
                            severity);
                    }
                }
                else if (currentStyle != style)
                {
                    yield return CreateLineViolation(
                        codeBlock.Line,
                        line,
                        $"Code fence style should be {style}",
                        severity);
                }
            }
        }
    }
}
