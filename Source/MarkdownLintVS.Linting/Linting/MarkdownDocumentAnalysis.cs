using System.Collections.Generic;
using System.Text.RegularExpressions;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace MarkdownLintVS.Linting
{
    /// <summary>
    /// Represents a parsed markdown document with helper methods for analysis.
    /// </summary>
    public class MarkdownDocumentAnalysis
    {
        // Default compiled pattern for front matter title detection
        private const string _defaultTitlePattern = @"^\s*title\s*[:=]";
        private static readonly Regex _defaultTitleRegex = new(
            _defaultTitlePattern,
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Patterns for TOC comment detection
        private static readonly Regex _tocStartPattern = new(
            @"<!--\s*TOC\s*-->",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex _tocEndPattern = new(
            @"<!--\s*/\s*TOC\s*-->",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Shared pipeline instance - thread-safe for parsing (configuration is immutable)
        private static readonly MarkdownPipeline _sharedPipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UsePreciseSourceLocation()
            .Build();

        private readonly string _text;
        private readonly string[] _lines;
        private readonly MarkdownDocument _document;

        // Precomputed caches for O(1) lookups
        private readonly int[] _lineStartOffsets;
        private readonly HashSet<int> _codeBlockLines;
        private readonly HashSet<int> _htmlBlockLines;
        private readonly int _frontMatterEndLine;
        private readonly HashSet<int> _tocCommentLines;
        private readonly SuppressionMap _suppressionMap;
        private readonly string _frontMatterRootPath;

        public string Text => _text;
        public string[] Lines => _lines;
        public MarkdownDocument Document => _document;
        public int LineCount => _lines.Length;

        /// <summary>
        /// Gets the file path of the document being analyzed, if available.
        /// Used by rules that need to resolve relative paths (e.g., MD061, MD062).
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Gets or sets the root path from .editorconfig (md_root_path setting).
        /// This is set by the analyzer after parsing EditorConfig.
        /// </summary>
        public string EditorConfigRootPath { get; set; }

        /// <summary>
        /// Gets or sets the root path from the VS Options page.
        /// This is set by the analyzer after reading options.
        /// </summary>
        public string OptionsRootPath { get; set; }

        /// <summary>
        /// Gets the effective root path for resolving root-relative paths.
        /// Precedence: front matter > .editorconfig > VS options.
        /// </summary>
        public string RootPath
        {
            get
            {
                // Front matter takes highest precedence
                if (!string.IsNullOrWhiteSpace(_frontMatterRootPath))
                    return _frontMatterRootPath;

                // Then .editorconfig
                if (!string.IsNullOrWhiteSpace(EditorConfigRootPath))
                    return EditorConfigRootPath;

                // Finally VS options
                if (!string.IsNullOrWhiteSpace(OptionsRootPath))
                    return OptionsRootPath;

                return null;
            }
        }

        /// <summary>
        /// Gets the suppression map containing inline suppression comments parsed from the document.
        /// </summary>
        public SuppressionMap Suppressions => _suppressionMap;

        public MarkdownDocumentAnalysis(string text) : this(text, null)
        {
        }

        public MarkdownDocumentAnalysis(string text, string filePath)
        {
            FilePath = filePath;
            _text = text ?? string.Empty;
            (_lines, _lineStartOffsets) = SplitLinesWithOffsets(_text);

            _document = Markdown.Parse(_text, _sharedPipeline);

            // Precompute expensive lookups once
            _codeBlockLines = BuildCodeBlockLinesCache();
            _htmlBlockLines = BuildHtmlBlockLinesCache();
            _frontMatterEndLine = ComputeFrontMatterEndLine();
            _tocCommentLines = BuildTocCommentLinesCache();
            _suppressionMap = BuildSuppressionMap();
            _frontMatterRootPath = ExtractRootPathFromFrontMatter();
        }

        /// <summary>
        /// Gets the root_path value from YAML front matter, if present.
        /// This is used by MD061/MD062 to resolve root-relative paths (starting with /).
        /// </summary>
        public string FrontMatterRootPath => _frontMatterRootPath;

        /// <summary>
        /// Extracts the root_path value from YAML front matter in the document.
        /// Uses the same approach as HasFrontMatterTitle for consistency.
        /// </summary>
        private string ExtractRootPathFromFrontMatter()
        {
            // No front matter means no root_path
            if (_frontMatterEndLine < 0)
                return null;

            // Search lines between front matter delimiters (excluding the --- lines)
            for (var i = 1; i < _frontMatterEndLine && i < _lines.Length; i++)
            {
                var lineText = _lines[i].Trim();

                // Look for root_path: value (case-insensitive)
                if (lineText.StartsWith("root_path:", StringComparison.OrdinalIgnoreCase))
                {
                    // Extract the value after the colon
                    var colonIndex = lineText.IndexOf(':');
                    if (colonIndex >= 0 && colonIndex < lineText.Length - 1)
                    {
                        var value = lineText.Substring(colonIndex + 1).Trim();

                        // Remove matching quotes if present (both single or double)
                        if (value.Length >= 2)
                        {
                            var firstChar = value[0];
                            var lastChar = value[value.Length - 1];

                            if ((firstChar == '"' && lastChar == '"') ||
                                (firstChar == '\'' && lastChar == '\''))
                            {
                                value = value.Substring(1, value.Length - 2);
                            }
                        }

                        return string.IsNullOrWhiteSpace(value) ? null : value;
                    }
                }
            }

            return null;
        }

        private SuppressionMap BuildSuppressionMap()
        {
            var parser = new SuppressionCommentParser();
            return parser.Parse(_lines);
        }

        private static (string[] Lines, int[] LineStartOffsets) SplitLinesWithOffsets(string text)
        {
            if (string.IsNullOrEmpty(text))
                return ([string.Empty], [0]);

            var lines = new List<string>();
            var offsets = new List<int>();
            var start = 0;

            offsets.Add(0); // First line starts at offset 0

            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    var end = i;
                    if (end > start && text[end - 1] == '\r')
                        end--;
                    lines.Add(text.Substring(start, end - start));
                    start = i + 1;
                    if (start <= text.Length)
                        offsets.Add(start);
                }
            }

            // Add the last line
            if (start <= text.Length)
            {
                var end = text.Length;
                if (end > start && text[end - 1] == '\r')
                    end--;
                lines.Add(text.Substring(start, end - start));
            }

            return ([.. lines], [.. offsets]);
        }

        private HashSet<int> BuildCodeBlockLinesCache()
        {
            var codeLines = new HashSet<int>();
            foreach (CodeBlock codeBlock in _document.Descendants<CodeBlock>())
            {
                var startLine = codeBlock.Line;
                var endLine = GetLineFromOffset(codeBlock.Span.End);
                for (var line = startLine; line <= endLine; line++)
                {
                    codeLines.Add(line);
                }
            }
            return codeLines;
        }

        private HashSet<int> BuildHtmlBlockLinesCache()
        {
            var htmlLines = new HashSet<int>();
            foreach (HtmlBlock htmlBlock in _document.Descendants<HtmlBlock>())
            {
                var startLine = htmlBlock.Line;
                var endLine = GetLineFromOffset(htmlBlock.Span.End);
                for (var line = startLine; line <= endLine; line++)
                {
                    htmlLines.Add(line);
                }
            }
            return htmlLines;
        }

        private int ComputeFrontMatterEndLine()
        {
            // Check for YAML front matter (starts with --- on line 0)
            if (_lines.Length > 0 && _lines[0].Trim() == "---")
            {
                for (var i = 1; i < _lines.Length; i++)
                {
                    if (_lines[i].Trim() == "---" || _lines[i].Trim() == "...")
                    {
                        return i;
                    }
                }
            }
            return -1; // No front matter
        }

        private HashSet<int> BuildTocCommentLinesCache()
        {
            var tocLines = new HashSet<int>();
            var inTocComment = false;

            for (var i = 0; i < _lines.Length; i++)
            {
                var line = _lines[i];

                if (!inTocComment && _tocStartPattern.IsMatch(line))
                {
                    inTocComment = true;
                    tocLines.Add(i);
                }
                else if (inTocComment)
                {
                    tocLines.Add(i);
                    if (_tocEndPattern.IsMatch(line))
                    {
                        inTocComment = false;
                    }
                }
            }

            return tocLines;
        }

        private int GetLineFromOffset(int offset)
        {
            if (offset < 0 || _lineStartOffsets.Length == 0)
                return 0;

            // Binary search for the line containing this offset
            var lo = 0;
            var hi = _lineStartOffsets.Length - 1;

            while (lo < hi)
            {
                var mid = lo + (hi - lo + 1) / 2;
                if (_lineStartOffsets[mid] <= offset)
                    lo = mid;
                else
                    hi = mid - 1;
            }

            return lo;
        }

        public string GetLine(int lineNumber)
        {
            if (lineNumber < 0 || lineNumber >= _lines.Length)
                return string.Empty;
            return _lines[lineNumber];
        }

        public IEnumerable<HeadingBlock> GetHeadings()
        {
            return _document.Descendants<HeadingBlock>();
        }

        public IEnumerable<FencedCodeBlock> GetFencedCodeBlocks()
        {
            return _document.Descendants<FencedCodeBlock>();
        }

        public IEnumerable<CodeBlock> GetCodeBlocks()
        {
            return _document.Descendants<CodeBlock>();
        }

        public IEnumerable<ListBlock> GetLists()
        {
            return _document.Descendants<ListBlock>();
        }

        public IEnumerable<ListItemBlock> GetListItems()
        {
            return _document.Descendants<ListItemBlock>();
        }

        public IEnumerable<QuoteBlock> GetBlockQuotes()
        {
            return _document.Descendants<QuoteBlock>();
        }

        public IEnumerable<ThematicBreakBlock> GetThematicBreaks()
        {
            return _document.Descendants<ThematicBreakBlock>();
        }

        public IEnumerable<LinkInline> GetLinks()
        {
            return _document.Descendants<LinkInline>();
        }

        public IEnumerable<EmphasisInline> GetEmphasis()
        {
            return _document.Descendants<EmphasisInline>();
        }

        public IEnumerable<CodeInline> GetCodeSpans()
        {
            return _document.Descendants<CodeInline>();
        }

        public IEnumerable<HtmlBlock> GetHtmlBlocks()
        {
            return _document.Descendants<HtmlBlock>();
        }

        public IEnumerable<HtmlInline> GetHtmlInlines()
        {
            return _document.Descendants<HtmlInline>();
        }

        public IEnumerable<Markdig.Extensions.Tables.Table> GetTables()
        {
            return _document.Descendants<Markdig.Extensions.Tables.Table>();
        }

        public IEnumerable<LinkReferenceDefinition> GetLinkReferenceDefinitions()
        {
            // LinkReferenceDefinitions in Markdig are stored in LinkReferenceDefinitionGroup blocks
            // which are children of the document
            foreach (Block block in _document)
            {
                // LinkReferenceDefinitionGroup contains individual LinkReferenceDefinition blocks
                if (block is LinkReferenceDefinitionGroup group)
                {
                    foreach (Block child in group)
                    {
                        if (child is LinkReferenceDefinition def)
                            yield return def;
                    }
                }
                // Individual LinkReferenceDefinition (rare but possible)
                else if (block is LinkReferenceDefinition def)
                {
                    yield return def;
                }
            }
        }

        public bool IsLineInCodeBlock(int lineNumber)
        {
            return _codeBlockLines.Contains(lineNumber);
        }

        /// <summary>
        /// Gets the code language for a line if it's inside a fenced code block.
        /// Returns null if the line is not in a fenced code block.
        /// </summary>
        public string GetCodeBlockLanguage(int lineNumber)
        {
            foreach (FencedCodeBlock codeBlock in GetFencedCodeBlocks())
            {
                var startLine = codeBlock.Line;
                var endLine = GetBlockEndLine(codeBlock);

                if (lineNumber >= startLine && lineNumber <= endLine)
                {
                    return codeBlock.Info?.ToLowerInvariant() ?? string.Empty;
                }
            }
            return null;
        }

        public bool IsLineInHtmlBlock(int lineNumber)
        {
            return _htmlBlockLines.Contains(lineNumber);
        }

        /// <summary>
        /// Checks if a line is inside a TOC (Table of Contents) HTML comment block.
        /// TOC comments are delimited by &lt;!--TOC--&gt; and &lt;!--/TOC--&gt; with optional whitespace.
        /// </summary>
        public bool IsLineInTocComment(int lineNumber)
        {
            return _tocCommentLines.Contains(lineNumber);
        }

        public bool IsLineInFrontMatter(int lineNumber)
        {
            return _frontMatterEndLine >= 0 && lineNumber >= 0 && lineNumber <= _frontMatterEndLine;
        }

        /// <summary>
        /// Checks if the front matter contains a title property matching the given pattern.
        /// </summary>
        /// <param name="titlePattern">Regex pattern to match title property (default: ^\s*title\s*[:=])</param>
        /// <returns>True if front matter contains a matching title property.</returns>
        public bool HasFrontMatterTitle(string titlePattern = null)
        {
            if (_frontMatterEndLine < 0)
                return false;

            // Use cached regex for default pattern, create new one only for custom patterns
            Regex pattern = string.IsNullOrEmpty(titlePattern) || titlePattern == _defaultTitlePattern
                ? _defaultTitleRegex
                : new Regex(titlePattern, RegexOptions.IgnoreCase);

            // Search lines between front matter delimiters (excluding the --- lines)
            for (var i = 1; i < _frontMatterEndLine && i < _lines.Length; i++)
            {
                if (pattern.IsMatch(_lines[i]))
                    return true;
            }

            return false;
        }

        public int GetBlockEndLine(Block block)
        {
            if (block.Span.End < 0)
                return block.Line;

            return GetLineFromOffset(block.Span.End);
        }

        public (int Line, int Column) GetPositionFromOffset(int offset)
        {
            if (offset < 0 || _lineStartOffsets.Length == 0)
                return (0, 0);

            var line = GetLineFromOffset(offset);
            var column = offset - _lineStartOffsets[line];

            return (line, column);
        }

        public int GetOffsetFromPosition(int line, int column)
        {
            if (line < 0 || line >= _lineStartOffsets.Length)
                return 0;

            return _lineStartOffsets[line] + column;
        }

        public bool IsBlankLine(int lineNumber)
        {
            if (lineNumber < 0 || lineNumber >= _lines.Length)
                return true;
            return string.IsNullOrWhiteSpace(_lines[lineNumber]);
        }

        /// <summary>
        /// Enumerates lines that should be analyzed, skipping code blocks and front matter by default.
        /// </summary>
        /// <param name="skipCodeBlocks">If true, skips lines inside code blocks.</param>
        /// <param name="skipFrontMatter">If true, skips lines inside YAML front matter.</param>
        /// <returns>Enumerable of (lineNumber, lineText) tuples for analyzable lines.</returns>
        public IEnumerable<(int LineNumber, string Line)> GetAnalyzableLines(
            bool skipCodeBlocks = true,
            bool skipFrontMatter = true)
        {
            for (var i = 0; i < LineCount; i++)
            {
                if (skipFrontMatter && IsLineInFrontMatter(i))
                    continue;

                if (skipCodeBlocks && IsLineInCodeBlock(i))
                    continue;

                yield return (i, GetLine(i));
            }
        }

        public int GetFirstNonBlankLine()
        {
            for (var i = 0; i < _lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(_lines[i]))
                    return i;
            }
            return -1;
        }

        public bool EndsWithNewline()
        {
            return _text.Length > 0 && _text[_text.Length - 1] == '\n';
        }

        public bool EndsWithMultipleNewlines()
        {
            if (_text.Length < 2)
                return false;

            var newlineCount = 0;
            for (var i = _text.Length - 1; i >= 0; i--)
            {
                if (_text[i] == '\n')
                    newlineCount++;
                else if (_text[i] != '\r')
                    break;
            }
            return newlineCount > 1;
        }

        /// <summary>
        /// Checks if a position (specified by line number and column) is inside an inline code span.
        /// </summary>
        /// <param name="lineNumber">The zero-based line number.</param>
        /// <param name="column">The zero-based column position within the line.</param>
        /// <returns>True if the position is inside an inline code span.</returns>
        public bool IsPositionInInlineCode(int lineNumber, int column)
        {
            var offset = GetOffsetFromPosition(lineNumber, column);

            foreach (CodeInline codeSpan in GetCodeSpans())
            {
                if (offset >= codeSpan.Span.Start && offset < codeSpan.Span.End)
                    return true;
            }

            return false;
        }
    }
}
