namespace MarkdownLintVS.Linting
{
    /// <summary>
    /// Holds the suppression state for a markdown document, tracking which rules
    /// are suppressed on which lines.
    /// </summary>
    public class SuppressionMap
    {
        // Lines where all rules are suppressed
        private readonly HashSet<int> _allRulesSuppressedLines;

        // Map of line number to set of suppressed rule IDs for that line
        private readonly Dictionary<int, HashSet<string>> _lineSuppressions;

        // Total line count for bounds checking
        private readonly int _lineCount;

        /// <summary>
        /// Creates a new SuppressionMap for a document with the given number of lines.
        /// </summary>
        /// <param name="lineCount">The total number of lines in the document.</param>
        public SuppressionMap(int lineCount)
        {
            _lineCount = lineCount;
            _allRulesSuppressedLines = [];
            _lineSuppressions = [];
        }

        /// <summary>
        /// Marks all rules as suppressed for the given line.
        /// </summary>
        /// <param name="lineNumber">The 0-based line number to suppress.</param>
        public void SuppressAllRules(int lineNumber)
        {
            if (lineNumber >= 0 && lineNumber < _lineCount)
            {
                _allRulesSuppressedLines.Add(lineNumber);
            }
        }

        /// <summary>
        /// Marks a specific rule as suppressed for the given line.
        /// </summary>
        /// <param name="lineNumber">The 0-based line number to suppress.</param>
        /// <param name="ruleId">The rule ID to suppress (e.g., "MD001" or "heading-increment").</param>
        public void SuppressRule(int lineNumber, string ruleId)
        {
            if (lineNumber < 0 || lineNumber >= _lineCount || string.IsNullOrEmpty(ruleId))
                return;

            if (!_lineSuppressions.TryGetValue(lineNumber, out HashSet<string> rules))
            {
                rules = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                _lineSuppressions[lineNumber] = rules;
            }

            rules.Add(ruleId);
        }

        /// <summary>
        /// Checks if a rule is suppressed for a given line.
        /// </summary>
        /// <param name="lineNumber">The 0-based line number to check.</param>
        /// <param name="ruleId">The rule ID to check (e.g., "MD001").</param>
        /// <param name="ruleName">The rule name to check (e.g., "heading-increment").</param>
        /// <param name="ruleAliases">Optional array of rule aliases to check.</param>
        /// <returns>True if the rule is suppressed on this line, false otherwise.</returns>
        public bool IsRuleSuppressed(int lineNumber, string ruleId, string ruleName = null)
        {
            // Check if all rules are suppressed for this line
            if (_allRulesSuppressedLines.Contains(lineNumber))
                return true;

            // Check if the specific rule is suppressed for this line
            if (_lineSuppressions.TryGetValue(lineNumber, out HashSet<string> suppressedRules))
            {
                // Check rule ID (e.g., "MD001")
                if (!string.IsNullOrEmpty(ruleId) && suppressedRules.Contains(ruleId))
                    return true;

                // Check rule name (e.g., "heading-increment")
                if (!string.IsNullOrEmpty(ruleName) && suppressedRules.Contains(ruleName))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if a rule is suppressed for a given line using a RuleInfo object.
        /// </summary>
        /// <param name="lineNumber">The 0-based line number to check.</param>
        /// <param name="rule">The RuleInfo containing rule ID, name, and aliases.</param>
        /// <returns>True if the rule is suppressed on this line, false otherwise.</returns>
        public bool IsRuleSuppressed(int lineNumber, RuleInfo rule)
        {
            if (rule == null)
                throw new ArgumentNullException(nameof(rule));

            return IsRuleSuppressed(lineNumber, rule.Id, rule.Name);
        }

        /// <summary>
        /// Checks if all rules are suppressed for a given line.
        /// </summary>
        /// <param name="lineNumber">The 0-based line number to check.</param>
        /// <returns>True if all rules are suppressed on this line, false otherwise.</returns>
        public bool AreAllRulesSuppressed(int lineNumber)
        {
            return _allRulesSuppressedLines.Contains(lineNumber);
        }

        /// <summary>
        /// Gets the set of suppressed rule IDs for a given line.
        /// </summary>
        /// <param name="lineNumber">The 0-based line number.</param>
        /// <returns>A read-only set of suppressed rule IDs, or an empty set if none.</returns>
        public IReadOnlyCollection<string> GetSuppressedRules(int lineNumber)
        {
            if (_lineSuppressions.TryGetValue(lineNumber, out HashSet<string> rules))
            {
                return rules;
            }
            return [];
        }

        /// <summary>
        /// Gets whether any suppressions exist in this map.
        /// </summary>
        public bool HasSuppressions => _allRulesSuppressedLines.Count > 0 || _lineSuppressions.Count > 0;
    }
}
