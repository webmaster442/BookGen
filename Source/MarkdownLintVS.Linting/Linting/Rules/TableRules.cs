using Markdig.Extensions.Tables;
using Markdig.Syntax;

namespace MarkdownLintVS.Linting.Rules
{
    /// <summary>
    /// MD055: Table pipe style.
    /// </summary>
    public class MD055_TablePipeStyle : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD055;
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

            foreach (Table table in analysis.GetTables())
            {
                for (var rowIndex = 0; rowIndex < table.Count; rowIndex++)
                {
                    Block row = table[rowIndex];
                    if (row is TableRow tableRow)
                    {
                        var lineNum = tableRow.Line;
                        var line = analysis.GetLine(lineNum);
                        var currentStyle = GetPipeStyle(line);

                        if (style == "consistent")
                        {
                            if (detectedStyle == null)
                            {
                                detectedStyle = currentStyle;
                            }
                            else if (currentStyle != detectedStyle)
                            {
                                yield return CreateLineViolation(
                                    lineNum,
                                    line,
                                    $"Table pipe style should be consistent (expected {detectedStyle})",
                                    severity);
                            }
                        }
                        else if (currentStyle != style)
                        {
                            yield return CreateLineViolation(
                                lineNum,
                                line,
                                $"Table pipe style should be {style}",
                                severity);
                        }
                    }
                }
            }
        }

        private string GetPipeStyle(string line)
        {
            var trimmed = line.Trim();
            var hasLeading = trimmed.StartsWith("|");
            var hasTrailing = trimmed.EndsWith("|");

            if (hasLeading && hasTrailing) return "leading_and_trailing";
            if (hasLeading) return "leading_only";
            if (hasTrailing) return "trailing_only";
            return "no_leading_or_trailing";
        }
    }

    /// <summary>
    /// MD056: Table column count.
    /// </summary>
    public class MD056_TableColumnCount : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD056;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            foreach (Table table in analysis.GetTables())
            {
                int? expectedColumns = null;
                var headerLine = -1;

                foreach (Block row in table)
                {
                    if (row is TableRow tableRow)
                    {
                        var columnCount = tableRow.Count;
                        var lineNum = tableRow.Line;

                        if (expectedColumns == null)
                        {
                            expectedColumns = columnCount;
                            headerLine = lineNum;

                            // Check the delimiter row (immediately after the header)
                            var delimiterLineNum = headerLine + 1;
                            if (delimiterLineNum < analysis.LineCount)
                            {
                                var delimiterLine = analysis.GetLine(delimiterLineNum);
                                var delimiterColumnCount = CountDelimiterColumns(delimiterLine);

                                if (delimiterColumnCount > 0 && delimiterColumnCount != expectedColumns)
                                {
                                    yield return CreateLineViolation(
                                        delimiterLineNum,
                                        delimiterLine,
                                        $"Table column count should be {expectedColumns} (found {delimiterColumnCount})",
                                        severity);
                                }
                            }
                        }
                        else if (columnCount != expectedColumns)
                        {
                            yield return CreateLineViolation(
                                lineNum,
                                analysis.GetLine(lineNum),
                                $"Table column count should be {expectedColumns} (found {columnCount})",
                                severity);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Counts the number of columns in a delimiter row by counting pipe-separated segments.
        /// Returns 0 if the line is not a valid delimiter row.
        /// </summary>
        private static int CountDelimiterColumns(string line)
        {
            var trimmed = line.Trim();
            if (trimmed.Length == 0)
                return 0;

            // Verify it looks like a delimiter row (only |, -, :, whitespace)
            if (!trimmed.All(c => c == '|' || c == '-' || c == ':' || char.IsWhiteSpace(c)))
                return 0;

            // Strip leading/trailing pipes then split on |
            if (trimmed.StartsWith("|"))
                trimmed = trimmed.Substring(1);
            if (trimmed.EndsWith("|"))
                trimmed = trimmed.Substring(0, trimmed.Length - 1);

            if (trimmed.Length == 0)
                return 0;

            // Count segments that contain at least one dash
            var segments = trimmed.Split('|');
            var count = 0;
            foreach (var segment in segments)
            {
                if (segment.Contains("-"))
                    count++;
            }

            return count;
        }
    }

    /// <summary>
    /// MD058: Tables should be surrounded by blank lines.
    /// </summary>
    public class MD058_BlanksAroundTables : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD058;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            foreach (Table table in analysis.GetTables())
            {
                var startLine = table.Line;
                var endLine = analysis.GetBlockEndLine(table);

                // Check line before
                if (startLine > 0 && !analysis.IsBlankLine(startLine - 1))
                {
                    yield return CreateLineViolation(
                        startLine,
                        analysis.GetLine(startLine),
                        "Tables should be surrounded by blank lines",
                        severity,
                        "Add blank line before table");
                }

                // Check line after
                if (endLine < analysis.LineCount - 1 && !analysis.IsBlankLine(endLine + 1))
                {
                    yield return CreateLineViolation(
                        endLine,
                        analysis.GetLine(endLine),
                        "Tables should be surrounded by blank lines",
                        severity,
                        "Add blank line after table");
                }
            }
        }
    }

    /// <summary>
    /// MD060: Table column style should be consistent.
    /// Each column should have consistent alignment across all rows.
    /// </summary>
    public class MD060_TableColumnStyle : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD060;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            foreach (Table table in analysis.GetTables())
            {
                // Get the column alignments from the table
                var columnAlignments = table.ColumnDefinitions?
                    .Select(c => c.Alignment)
                    .ToList();

                if (columnAlignments == null || columnAlignments.Count == 0)
                    continue;

                // Check each row for alignment consistency
                foreach (Block row in table)
                {
                    if (row is TableRow tableRow)
                    {
                        var lineNum = tableRow.Line;
                        var line = analysis.GetLine(lineNum);

                        // Skip delimiter row (contains ---, :---, ---:, :---:)
                        if (IsDelimiterRow(line))
                            continue;

                        // Check if cell content alignment matches column alignment
                        var cellIndex = 0;
                        foreach (Block cell in tableRow)
                        {
                            if (cell is TableCell tableCell && cellIndex < columnAlignments.Count)
                            {
                                TableColumnAlign? expectedAlignment = columnAlignments[cellIndex];
                                var cellContent = GetCellContent(tableCell, analysis);
                                TableColumnAlign actualAlignment = DetectCellContentAlignment(cellContent);

                                // Only report if there's an explicit alignment defined and content doesn't match
                                if (expectedAlignment.HasValue &&
                                    expectedAlignment.Value != TableColumnAlign.Left &&
                                    actualAlignment != TableColumnAlign.Left &&
                                    expectedAlignment.Value != actualAlignment)
                                {
                                    yield return CreateViolation(
                                        lineNum,
                                        0,
                                        line.Length,
                                        $"Cell content alignment does not match column alignment (expected {GetAlignmentName(expectedAlignment.Value)})",
                                        severity,
                                        "Adjust cell content alignment");
                                    break; // Only report once per row
                                }
                            }
                            cellIndex++;
                        }
                    }
                }
            }
        }

        private static bool IsDelimiterRow(string line)
        {
            // Delimiter rows contain only |, -, :, and whitespace
            var trimmed = line.Trim();
            return trimmed.All(c => c == '|' || c == '-' || c == ':' || char.IsWhiteSpace(c));
        }

        private static string GetCellContent(TableCell cell, MarkdownDocumentAnalysis analysis)
        {
            if (cell.Span.Length <= 0)
                return string.Empty;

            var text = analysis.Text;
            var start = cell.Span.Start;
            var end = cell.Span.End;

            if (start >= 0 && end < text.Length && end >= start)
            {
                return text.Substring(start, end - start + 1).Trim();
            }

            return string.Empty;
        }

        private static TableColumnAlign DetectCellContentAlignment(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return TableColumnAlign.Left;

            // Check for leading/trailing whitespace patterns
            var leadingSpaces = content.Length - content.TrimStart().Length;
            var trailingSpaces = content.Length - content.TrimEnd().Length;

            if (leadingSpaces > 0 && trailingSpaces > 0 &&
                System.Math.Abs(leadingSpaces - trailingSpaces) <= 1)
            {
                return TableColumnAlign.Center;
            }

            if (leadingSpaces > trailingSpaces)
            {
                return TableColumnAlign.Right;
            }

            return TableColumnAlign.Left;
        }

        private static string GetAlignmentName(TableColumnAlign alignment)
        {
            return alignment switch
            {
                TableColumnAlign.Left => "left",
                TableColumnAlign.Center => "center",
                TableColumnAlign.Right => "right",
                _ => "left"
            };
        }
    }
}
