using System.Text.RegularExpressions;

using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace MarkdownLintVS.Linting.Rules
{
    /// <summary>
    /// MD049: Emphasis style should be consistent.
    /// </summary>
    public class MD049_EmphasisStyle : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD049;
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

            // Filter to only emphasis (* or _), excluding subscript (~), superscript (^), and highlight (=)
            foreach (EmphasisInline emphasis in analysis.GetEmphasis().Where(e => e.DelimiterCount == 1 && (e.DelimiterChar == '*' || e.DelimiterChar == '_')))
            {
                (var Line, var Column) = analysis.GetPositionFromOffset(emphasis.Span.Start);
                var line = analysis.GetLine(Line);

                if (Column >= line.Length) continue;

                var currentStyle = line[Column] == '*' ? "asterisk" : "underscore";

                if (style == "consistent")
                {
                    if (detectedStyle == null)
                    {
                        detectedStyle = currentStyle;
                    }
                    else if (currentStyle != detectedStyle)
                    {
                        yield return CreateViolation(
                            Line,
                            Column,
                            Column + emphasis.Span.Length,
                            $"Emphasis style should be consistent (expected {detectedStyle})",
                            severity);
                    }
                }
                else if (currentStyle != style)
                {
                    yield return CreateViolation(
                        Line,
                        Column,
                        Column + emphasis.Span.Length,
                        $"Emphasis style should be {style}",
                        severity);
                }
            }
        }
    }

    /// <summary>
    /// MD050: Strong style should be consistent.
    /// </summary>
    public class MD050_StrongStyle : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD050;
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

            // Filter to only strong emphasis (* or _), excluding strikethrough (~)
            foreach (EmphasisInline emphasis in analysis.GetEmphasis().Where(e => e.DelimiterCount == 2 && (e.DelimiterChar == '*' || e.DelimiterChar == '_')))
            {
                (var Line, var Column) = analysis.GetPositionFromOffset(emphasis.Span.Start);
                var line = analysis.GetLine(Line);

                if (Column >= line.Length) continue;

                var currentStyle = emphasis.DelimiterChar == '*' ? "asterisk" : "underscore";

                if (style == "consistent")
                {
                    if (detectedStyle == null)
                    {
                        detectedStyle = currentStyle;
                    }
                    else if (currentStyle != detectedStyle)
                    {
                        yield return CreateViolation(
                            Line,
                            Column,
                            Column + emphasis.Span.Length,
                            $"Strong style should be consistent (expected {detectedStyle})",
                            severity);
                    }
                }
                else if (currentStyle != style)
                {
                    yield return CreateViolation(
                        Line,
                        Column,
                        Column + emphasis.Span.Length,
                        $"Strong style should be {style}",
                        severity);
                }
            }
        }
    }

    /// <summary>
    /// MD051: Link fragments should be valid.
    /// </summary>
    public class MD051_LinkFragments : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD051;
        public override RuleInfo Info => _info;

        private static readonly Regex _nonWordPattern = new(
            @"[^\w\s-]",
            RegexOptions.Compiled);

        private static readonly Regex _whitespacePattern = new(
            @"\s+",
            RegexOptions.Compiled);

        // Match id attributes in HTML: id="value", id='value', or id=value
        private static readonly Regex _htmlIdPattern = new(
            @"(?:id|name)\s*=\s*[""']?([^""'\s>]+)[""']?",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Issue #726: kramdown block IAL pattern: {:#id} or {:.class #id} or {: #id .class}
        // Can appear on its own line after a block, or inline at end of heading
        private static readonly Regex _kramdownIalIdPattern = new(
            @"\{:\s*(?:[^}]*\s)?#([^\s}]+)(?:\s[^}]*)?\}",
            RegexOptions.Compiled);

        // kramdown heading IAL: ## Heading {:#custom-id}
        private static readonly Regex _kramdownHeadingIalPattern = new(
            @"\{:#([^\s}]+)\}\s*$",
            RegexOptions.Compiled);

        // Extended Markdown heading ID: ## Heading {#custom-id}
        // Used by GitHub, GitLab, Hugo, VitePress, Pandoc
        // See: https://www.markdownlang.com/extended/heading-ids.html
        private static readonly Regex _extendedHeadingIdPattern = new(
            @"\{#([^\s}]+)\}\s*$",
            RegexOptions.Compiled);

        // Extended Markdown block IAL pattern: {#id} or {.class #id} or {#id .class}
        private static readonly Regex _extendedIalIdPattern = new(
            @"\{(?:[^}]*\s)?#([^\s}]+)(?:\s[^}]*)?\}",
            RegexOptions.Compiled);

        // Inline markdown links: [text](url) or ![alt](url) — extract the text/alt portion
        private static readonly Regex _inlineLinkPattern = new(
            @"!?\[([^\]]*)\]\([^)]*\)",
            RegexOptions.Compiled);

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            // Collect all heading IDs
            var headingIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Add IDs from markdown headings
            foreach (HeadingBlock heading in analysis.GetHeadings())
            {
                var line = analysis.GetLine(heading.Line);

                // Check for kramdown heading IAL: ## Heading {:#custom-id}
                Match kramdownMatch = _kramdownHeadingIalPattern.Match(line);
                // Check for extended Markdown heading ID: ## Heading {#custom-id}
                Match extendedMatch = _extendedHeadingIdPattern.Match(line);

                if (kramdownMatch.Success)
                {
                    headingIds.Add(kramdownMatch.Groups[1].Value);
                    // Remove the IAL from content for generating default ID
                    var contentWithoutIal = _kramdownHeadingIalPattern.Replace(line, "");
                    var content = contentWithoutIal.TrimStart('#', ' ').TrimEnd('#', ' ');
                    var id = CreateHeadingId(content);
                    if (!string.IsNullOrEmpty(id))
                    {
                        headingIds.Add(id);
                    }
                }
                else if (extendedMatch.Success)
                {
                    // Extended Markdown syntax: {#custom-id} (without colon)
                    headingIds.Add(extendedMatch.Groups[1].Value);
                    // Remove the ID from content for generating default ID
                    var contentWithoutId = _extendedHeadingIdPattern.Replace(line, "");
                    var content = contentWithoutId.TrimStart('#', ' ').TrimEnd('#', ' ');
                    var id = CreateHeadingId(content);
                    if (!string.IsNullOrEmpty(id))
                    {
                        headingIds.Add(id);
                    }
                }
                else
                {
                    var content = GetHeadingContent(heading.Line, analysis);
                    var id = CreateHeadingId(content);
                    headingIds.Add(id);
                }

                // Also check for explicit id in heading line (e.g., ## <a id="custom">Heading</a>)
                foreach (Match match in _htmlIdPattern.Matches(line))
                {
                    headingIds.Add(match.Groups[1].Value);
                }
            }

            // Scan all lines for HTML id/name attributes and IAL IDs
            for (var i = 0; i < analysis.LineCount; i++)
            {
                var line = analysis.GetLine(i);

                // HTML id/name attributes
                foreach (Match match in _htmlIdPattern.Matches(line))
                {
                    headingIds.Add(match.Groups[1].Value);
                }

                // Issue #726: kramdown block/span IAL IDs {:#id} or {:.class #id}
                foreach (Match match in _kramdownIalIdPattern.Matches(line))
                {
                    headingIds.Add(match.Groups[1].Value);
                }

                // Extended Markdown IAL IDs {#id} or {.class #id}
                foreach (Match match in _extendedIalIdPattern.Matches(line))
                {
                    headingIds.Add(match.Groups[1].Value);
                }
            }

            // Check all links with fragments
            foreach (LinkInline link in analysis.GetLinks())
            {
                if (string.IsNullOrEmpty(link.Url))
                    continue;

                // Only check internal fragment links
                if (link.Url.StartsWith("#"))
                {
                    var fragment = link.Url.Substring(1);

                    if (!string.IsNullOrEmpty(fragment) && !headingIds.Contains(fragment))
                    {
                        (var Line, var Column) = analysis.GetPositionFromOffset(link.Span.Start);
                        yield return CreateViolation(
                            Line,
                            Column,
                            Column + link.Span.Length,
                            $"Link fragment '#{fragment}' does not match any heading or anchor",
                            severity);
                    }
                }
            }
        }

        private static string GetHeadingContent(int lineNumber, MarkdownDocumentAnalysis analysis)
        {
            var line = analysis.GetLine(lineNumber);
            var content = line.TrimStart('#', ' ').TrimEnd('#', ' ');
            // Strip inline link/image syntax so [text](url) becomes just text
            content = _inlineLinkPattern.Replace(content, "$1");
            return content;
        }

        private static string CreateHeadingId(string content)
        {
            // Convert to lowercase, replace spaces with hyphens, remove special chars
            var id = content.ToLowerInvariant();
            id = _nonWordPattern.Replace(id, "");
            id = _whitespacePattern.Replace(id, "-");
            return id;
        }
    }

    /// <summary>
    /// MD052: Reference links and images should use a label that is defined.
    /// </summary>
    public class MD052_ReferenceLinksImages : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD052;
        public override RuleInfo Info => _info;

        private static readonly Regex _refLinkPattern = new(
            @"\[([^\]]+)\]\[([^\]]*)\]",
            RegexOptions.Compiled);

        private static readonly Regex _shortcutPattern = new(
            @"\[([^\]]+)\](?!\()",
            RegexOptions.Compiled);

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            var shortcutSyntax = configuration.GetBoolParameter("shortcut_syntax", true);

            // Collect all defined labels
            var definedLabels = new HashSet<string>(
                analysis.GetLinkReferenceDefinitions()
                    .Where(d => d.Label != null)
                    .Select(d => d.Label.ToLowerInvariant()));

            for (var i = 0; i < analysis.LineCount; i++)
            {
                if (analysis.IsLineInCodeBlock(i) || analysis.IsLineInFrontMatter(i))
                    continue;

                var line = analysis.GetLine(i);

                // Full reference: [text][label]
                foreach (Match match in _refLinkPattern.Matches(line))
                {
                    var label = match.Groups[2].Value;
                    if (string.IsNullOrEmpty(label))
                        label = match.Groups[1].Value; // Collapsed reference

                    if (!definedLabels.Contains(label.ToLowerInvariant()))
                    {
                        yield return CreateViolation(
                            i,
                            match.Index,
                            match.Index + match.Length,
                            $"Reference link label '{label}' is not defined",
                            severity);
                    }
                }

                // Shortcut reference: [label]
                if (shortcutSyntax)
                {
                    foreach (Match match in _shortcutPattern.Matches(line))
                    {
                        var label = match.Groups[1].Value;

                        // Skip if it's part of a full reference [text][label] - check if followed by [
                        if (line.Length > match.Index + match.Length && line[match.Index + match.Length] == '[')
                            continue;

                        // Skip if it's the label part of a full reference [text][label] - check if preceded by ]
                        if (match.Index > 0 && line[match.Index - 1] == ']')
                            continue;

                        // Skip task list checkboxes: [ ], [x], [X]
                        if (label == " " || label.Equals("x", StringComparison.OrdinalIgnoreCase))
                            continue;

                        // Skip common false positive patterns to avoid flagging valid markdown
                        if (LooksLikeNonReference(label))
                            continue;

                        if (!definedLabels.Contains(label.ToLowerInvariant()))
                        {
                            yield return CreateViolation(
                                i,
                                match.Index,
                                match.Index + match.Length,
                                $"Reference link label '{label}' is not defined",
                                severity);
                        }
                    }
                }
            }
        }

        private static readonly Regex _numericPattern = new(@"^\d+$", RegexOptions.Compiled);
        private static readonly Regex _singleWordPattern = new(@"^\w+$", RegexOptions.Compiled);
        private static readonly HashSet<string> _keyboardKeys = new(StringComparer.OrdinalIgnoreCase)
        {
            "Ctrl", "Alt", "Shift", "Tab", "Enter", "Esc", "Escape", "Space",
            "Backspace", "Delete", "Del", "Insert", "Ins", "Home", "End",
            "PageUp", "PageDown", "PgUp", "PgDn", "Up", "Down", "Left", "Right",
            "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12",
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
            "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
        };

        private static bool LooksLikeNonReference(string label)
        {
            // Skip numeric labels like [1], [2] (footnote-style)
            if (_numericPattern.IsMatch(label))
                return true;

            // Skip keyboard key names like [Ctrl], [Alt], [A], [F1]
            if (_keyboardKeys.Contains(label))
                return true;

            // Skip single words without hyphens like [what], [example], [note]
            // These are typically bracketed text, not intended reference links
            // Real reference links usually have hyphens like [my-link] or multiple words
            if (_singleWordPattern.IsMatch(label))
                return true;

            return false;
        }
    }

    /// <summary>
    /// MD053: Link and image reference definitions should be needed.
    /// </summary>
    public class MD053_LinkImageReferenceDefinitions : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD053;
        public override RuleInfo Info => _info;

        private static readonly Regex _refLinkUsagePattern = new(
            @"\[([^\]]+)\]\[([^\]]*)\]|\[([^\]]+)\](?!\()",
            RegexOptions.Compiled);

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            var ignoredDefinitions = configuration.GetStringParameter("ignored_definitions", "//")
                .Split(',')
                .Select(d => d.Trim().ToLowerInvariant())
                .Where(d => !string.IsNullOrEmpty(d))
                .ToHashSet();

            // Precompile regex patterns for ignored definitions that use regex syntax
            var ignoredRegexPatterns = ignoredDefinitions
                .Where(ignored => ignored.StartsWith("/") && ignored.EndsWith("/") && ignored.Length > 2)
                .Select(ignored => new Regex(ignored.Substring(1, ignored.Length - 2), RegexOptions.Compiled))
                .ToList();

            var ignoredLiterals = ignoredDefinitions
                .Where(ignored => !(ignored.StartsWith("/") && ignored.EndsWith("/") && ignored.Length > 2))
                .ToHashSet();

            // Collect all used labels
            var usedLabels = new HashSet<string>();

            for (var i = 0; i < analysis.LineCount; i++)
            {
                if (analysis.IsLineInCodeBlock(i))
                    continue;

                var line = analysis.GetLine(i);
                foreach (Match match in _refLinkUsagePattern.Matches(line))
                {
                    string label;
                    if (match.Groups[3].Success)
                        label = match.Groups[3].Value;
                    else if (!string.IsNullOrEmpty(match.Groups[2].Value))
                        label = match.Groups[2].Value;
                    else
                        label = match.Groups[1].Value;

                    usedLabels.Add(label.ToLowerInvariant());
                }
            }

            // Check definitions
            foreach (LinkReferenceDefinition definition in analysis.GetLinkReferenceDefinitions())
            {
                if (definition.Label == null)
                    continue;

                var label = definition.Label.ToLowerInvariant();

                // Check if ignored by literal match
                if (ignoredLiterals.Contains(label))
                    continue;

                // Check if ignored by regex pattern
                if (ignoredRegexPatterns.Any(pattern => pattern.IsMatch(label)))
                    continue;

                if (!usedLabels.Contains(label))
                {
                    yield return CreateLineViolation(
                        definition.Line,
                        analysis.GetLine(definition.Line),
                        $"Link reference definition '{label}' is not used",
                        severity,
                        "Remove unused definition");
                }
            }
        }
    }

    /// <summary>
    /// MD059: Link text should be descriptive.
    /// Flags links with generic text like "click here", "read more", "here", etc.
    /// </summary>
    public class MD059_DescriptiveLinkText : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD059;
        public override RuleInfo Info => _info;

        // Non-descriptive link text patterns (case-insensitive)
        private static readonly HashSet<string> _nonDescriptiveTexts = new(StringComparer.OrdinalIgnoreCase)
        {
            "click here",
            "click",
            "here",
            "link",
            "more",
            "read more",
            "learn more",
            "this",
            "this link",
            "page",
            "this page",
            "article",
            "this article",
            "go here",
            "go",
            "details",
            "more details",
            "info",
            "more info",
            "more information",
            "see here",
            "see more",
            "full article",
            "continue reading",
            "continue",
            "read",
            "edit",
            "source",
            "view",
            "view source",
            "download",
            "download here"
        };

        // Patterns that are just URLs
        private static readonly Regex _urlPattern = new(
            @"^https?://",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            // Get additional allowed texts from configuration
            var allowedTexts = configuration.GetStringParameter("allowed_texts", "")
                .Split(',')
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrEmpty(t))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (LinkInline link in analysis.GetLinks())
            {
                // Get link text content
                var linkText = GetLinkText(link);

                if (string.IsNullOrWhiteSpace(linkText))
                    continue;

                var normalizedText = linkText.Trim();

                // Skip if explicitly allowed
                if (allowedTexts.Contains(normalizedText))
                    continue;

                // Check if text is non-descriptive
                var isNonDescriptive = _nonDescriptiveTexts.Contains(normalizedText);

                // Also flag URLs used as link text
                if (!isNonDescriptive && _urlPattern.IsMatch(normalizedText))
                {
                    isNonDescriptive = true;
                }

                if (isNonDescriptive)
                {
                    (var Line, var Column) = analysis.GetPositionFromOffset(link.Span.Start);
                    yield return CreateViolation(
                        Line,
                        Column,
                        Column + link.Span.Length,
                        $"Link text '{normalizedText}' is not descriptive",
                        severity);
                }
            }
        }

        private static string GetLinkText(LinkInline link)
        {
            // Extract text content from the link's children
            var text = new System.Text.StringBuilder();

            foreach (Inline inline in link)
            {
                if (inline is LiteralInline literal)
                {
                    text.Append(literal.Content.ToString());
                }
                else if (inline is EmphasisInline emphasis)
                {
                    foreach (Inline child in emphasis)
                    {
                        if (child is LiteralInline literalChild)
                        {
                            text.Append(literalChild.Content.ToString());
                        }
                    }
                }
            }

            return text.ToString();
        }
    }
}

