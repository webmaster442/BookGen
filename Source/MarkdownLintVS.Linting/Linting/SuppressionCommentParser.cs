using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownLintVS.Linting
{
    /// <summary>
    /// Parses markdownlint inline suppression comments from markdown documents.
    /// Supports all standard markdownlint comment patterns:
    /// - disable/enable (toggle all rules)
    /// - disable RULES/enable RULES (toggle specific rules)
    /// - disable-line/disable-line RULES (suppress current line)
    /// - disable-next-line/disable-next-line RULES (suppress next line)
    /// - capture/restore (scoped suppression)
    /// - disable-file/disable-file RULES (file-level suppression)
    /// - configure-file (inline configuration - treated as suppress)
    /// 
    /// Optimized to use a single pass through the document for better performance.
    /// </summary>
    public class SuppressionCommentParser
    {
        // Pattern to match HTML comments containing markdownlint directives
        // Matches: <!-- markdownlint-DIRECTIVE OPTIONAL_RULES -->
        private static readonly Regex _commentPattern = new(
            @"<!--\s*markdownlint-(disable|enable|disable-line|disable-next-line|capture|restore|disable-file|configure-file)(?:\s+([^>]+?))?\s*-->",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Pattern to extract rule identifiers from the rules portion
        // Matches: MD001, md001, heading-increment, etc.
        private static readonly Regex _rulePattern = new(
            @"(MD\d{3}|[a-zA-Z][a-zA-Z0-9_-]*)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Represents a parsed directive from a line.
        /// </summary>
        private readonly struct ParsedDirective(string directive, string rulesText, int lineNumber)
        {
            public readonly string Directive = directive;
            public readonly string RulesText = rulesText;
            public readonly int LineNumber = lineNumber;
        }

        /// <summary>
        /// Parses all suppression comments from the given lines and returns a SuppressionMap.
        /// Uses a single pass for optimal performance.
        /// </summary>
        /// <param name="lines">The lines of the markdown document.</param>
        /// <returns>A SuppressionMap containing all parsed suppressions.</returns>
        public SuppressionMap Parse(string[] lines)
        {
            if (lines == null)
                throw new ArgumentNullException(nameof(lines));

            var map = new SuppressionMap(lines.Length);

            // Single pass: parse all directives and collect file-level ones
            var directives = new List<ParsedDirective>(lines.Length / 10); // Estimate ~10% of lines have directives
            var fileLevelSuppressions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var fileLevelDisableAll = false;

            // Parse all lines in one pass
            for (var lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                var line = lines[lineNumber];
                Match match = _commentPattern.Match(line);

                if (!match.Success)
                    continue;

                var directive = match.Groups[1].Value.ToLowerInvariant();
                var rulesText = match.Groups[2].Success ? match.Groups[2].Value : null;

                // Check for file-level suppressions immediately
                switch (directive)
                {
                    case "disable-file":
                        if (string.IsNullOrWhiteSpace(rulesText))
                        {
                            fileLevelDisableAll = true;
                        }
                        else
                        {
                            HashSet<string> rules = ParseRules(rulesText);
                            foreach (var rule in rules)
                            {
                                fileLevelSuppressions.Add(rule);
                            }
                        }
                        break;

                    case "configure-file":
                        if (!string.IsNullOrWhiteSpace(rulesText))
                        {
                            HashSet<string> rules = ParseRules(rulesText);
                            foreach (var rule in rules)
                            {
                                fileLevelSuppressions.Add(rule);
                            }
                        }
                        break;

                    default:
                        // Store non-file-level directives for processing
                        directives.Add(new ParsedDirective(directive, rulesText, lineNumber));
                        break;
                }
            }

            // If file-level disable-all is active, suppress all lines and return early
            if (fileLevelDisableAll)
            {
                for (var i = 0; i < lines.Length; i++)
                {
                    map.SuppressAllRules(i);
                }
                return map;
            }

            // Apply file-level rule suppressions to all lines
            if (fileLevelSuppressions.Count > 0)
            {
                for (var i = 0; i < lines.Length; i++)
                {
                    foreach (var rule in fileLevelSuppressions)
                    {
                        map.SuppressRule(i, rule);
                    }
                }
            }

            // Process scoped suppressions using the cached directives
            ProcessScopedSuppressions(lines.Length, directives, map);

            return map;
        }

        /// <summary>
        /// Process scoped suppressions using pre-parsed directives.
        /// </summary>
        private void ProcessScopedSuppressions(int lineCount, List<ParsedDirective> directives, SuppressionMap map)
        {
            if (directives.Count == 0)
                return;

            // Track disable/enable state for scoped suppressions
            var activeDisables = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var disableAllActive = false;

            // Stack for capture/restore
            var captureStack = new Stack<(bool DisableAllActive, HashSet<string> ActiveDisables)>();

            var directiveIndex = 0;
            var nextDirectiveLine = directives[0].LineNumber;

            for (var lineNumber = 0; lineNumber < lineCount; lineNumber++)
            {
                // Check if current line has a directive
                if (lineNumber == nextDirectiveLine && directiveIndex < directives.Count)
                {
                    ParsedDirective parsed = directives[directiveIndex];
                    HashSet<string> rules = ParseRules(parsed.RulesText);

                    switch (parsed.Directive)
                    {
                        case "disable":
                            if (rules.Count == 0)
                            {
                                disableAllActive = true;
                            }
                            else
                            {
                                foreach (var rule in rules)
                                {
                                    activeDisables.Add(rule);
                                }
                            }
                            break;

                        case "enable":
                            if (rules.Count == 0)
                            {
                                disableAllActive = false;
                                activeDisables.Clear();
                            }
                            else
                            {
                                foreach (var rule in rules)
                                {
                                    activeDisables.Remove(rule);
                                }
                            }
                            break;

                        case "disable-line":
                            // Suppress this line only
                            if (rules.Count == 0)
                            {
                                map.SuppressAllRules(lineNumber);
                            }
                            else
                            {
                                foreach (var rule in rules)
                                {
                                    map.SuppressRule(lineNumber, rule);
                                }
                            }
                            break;

                        case "disable-next-line":
                            // Suppress the next line only
                            var nextLine = lineNumber + 1;
                            if (nextLine < lineCount)
                            {
                                if (rules.Count == 0)
                                {
                                    map.SuppressAllRules(nextLine);
                                }
                                else
                                {
                                    foreach (var rule in rules)
                                    {
                                        map.SuppressRule(nextLine, rule);
                                    }
                                }
                            }
                            break;

                        case "capture":
                            // Save current state to stack
                            captureStack.Push((disableAllActive, new HashSet<string>(activeDisables, StringComparer.OrdinalIgnoreCase)));
                            break;

                        case "restore":
                            // Restore previous state from stack
                            if (captureStack.Count > 0)
                            {
                                (var savedDisableAll, HashSet<string> savedDisables) = captureStack.Pop();
                                disableAllActive = savedDisableAll;
                                activeDisables = savedDisables;
                            }
                            else
                            {
                                // No capture to restore - reset to default state
                                disableAllActive = false;
                                activeDisables.Clear();
                            }
                            break;
                    }

                    // Move to next directive
                    directiveIndex++;
                    nextDirectiveLine = directiveIndex < directives.Count ? directives[directiveIndex].LineNumber : int.MaxValue;
                }

                // Apply current suppression state to this line
                if (disableAllActive)
                {
                    map.SuppressAllRules(lineNumber);
                }
                else if (activeDisables.Count > 0)
                {
                    foreach (var rule in activeDisables)
                    {
                        map.SuppressRule(lineNumber, rule);
                    }
                }
            }
        }

        /// <summary>
        /// Parses rule identifiers from a comma/space-separated string.
        /// </summary>
        private static HashSet<string> ParseRules(string rulesText)
        {
            var rules = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(rulesText))
                return rules;

            MatchCollection matches = _rulePattern.Matches(rulesText);
            foreach (Match match in matches)
            {
                var rule = match.Value;
                rules.Add(NormalizeRuleId(rule));
            }

            return rules;
        }

        /// <summary>
        /// Normalizes a rule identifier to uppercase for MD### format.
        /// </summary>
        private static string NormalizeRuleId(string ruleId)
        {
            if (ruleId.StartsWith("MD", StringComparison.OrdinalIgnoreCase) ||
                ruleId.StartsWith("md", StringComparison.OrdinalIgnoreCase))
            {
                return ruleId.ToUpperInvariant();
            }
            return ruleId.ToLowerInvariant();
        }
    }
}
