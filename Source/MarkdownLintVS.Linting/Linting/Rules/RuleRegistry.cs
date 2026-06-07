namespace MarkdownLintVS.Linting.Rules
{
    internal static class RuleRegistry
    {
        // ===== Heading Rules =====
        public static RuleInfo MD001 = new("MD001", "heading-increment", "Heading levels should only increment by one level at a time");

        public static RuleInfo MD003 = new("MD003", "heading-style", "Heading style should be consistent");

        public static RuleInfo MD018 = new("MD018", "no-missing-space-atx", "No space after hash on atx style heading");

        public static RuleInfo MD019 = new("MD019", "no-multiple-space-atx", "Multiple spaces after hash on atx style heading");

        public static RuleInfo MD020 = new("MD020", "no-missing-space-closed-atx", "No space inside hashes on closed atx style heading");

        public static RuleInfo MD021 = new("MD021", "no-multiple-space-closed-atx", "Multiple spaces inside hashes on closed atx style heading");

        public static RuleInfo MD022 = new("MD022", "blanks-around-headings", "Headings should be surrounded by blank lines");

        public static RuleInfo MD023 = new("MD023", "heading-start-left", "Headings must start at the beginning of the line");

        public static RuleInfo MD024 = new("MD024", "no-duplicate-heading", "Multiple headings with the same content");

        public static RuleInfo MD025 = new("MD025", "single-title", "Multiple top-level headings in the same document");

        public static RuleInfo MD026 = new("MD026", "no-trailing-punctuation", "Trailing punctuation in heading");

        public static RuleInfo MD041 = new("MD041", "first-line-heading", "First line in a file should be a top-level heading");

        // ===== List Rules =====

        public static RuleInfo MD004 = new("MD004", "ul-style", "Unordered list style should be consistent");

        public static RuleInfo MD005 = new("MD005", "list-indent", "Inconsistent indentation for list items at the same level");

        public static RuleInfo MD007 = new("MD007", "ul-indent", "Unordered list indentation");

        public static RuleInfo MD029 = new("MD029", "ol-prefix", "Ordered list item prefix");

        public static RuleInfo MD030 = new("MD030", "list-marker-space", "Spaces after list markers");

        public static RuleInfo MD032 = new("MD032", "blanks-around-lists", "Lists should be surrounded by blank lines");

        // ===== Whitespace Rules =====

        public static RuleInfo MD009 = new("MD009", "no-trailing-spaces", "Trailing spaces");

        public static RuleInfo MD010 = new("MD010", "no-hard-tabs", "Hard tabs");

        public static RuleInfo MD012 = new("MD012", "no-multiple-blanks", "Multiple consecutive blank lines");

        public static RuleInfo MD013 = new("MD013", "line-length", "Line length");

        public static RuleInfo MD047 = new("MD047", "single-trailing-newline", "Files should end with a single newline character");

        // ===== Code Block Rules =====

        public static RuleInfo MD014 = new("MD014", "commands-show-output", "Dollar signs used before commands without showing output");

        public static RuleInfo MD031 = new("MD031", "blanks-around-fences", "Fenced code blocks should be surrounded by blank lines");

        public static RuleInfo MD040 = new("MD040", "fenced-code-language", "Fenced code blocks should have a language specified");

        public static RuleInfo MD046 = new("MD046", "code-block-style", "Code block style");

        public static RuleInfo MD048 = new("MD048", "code-fence-style", "Code fence style");

        // ===== Link Rules =====

        public static RuleInfo MD011 = new("MD011", "no-reversed-links", "Reversed link syntax");

        public static RuleInfo MD034 = new("MD034", "no-bare-urls", "Bare URL used");

        public static RuleInfo MD039 = new("MD039", "no-space-in-links", "Spaces inside link text");

        public static RuleInfo MD042 = new("MD042", "no-empty-links", "No empty links");

        public static RuleInfo MD045 = new("MD045", "no-alt-text", "Images should have alternate text (alt text)");

        public static RuleInfo MD051 = new("MD051", "link-fragments", "Link fragments should be valid");

        public static RuleInfo MD052 = new("MD052", "reference-links-images", "Reference links and images should use a label that is defined");

        public static RuleInfo MD053 = new("MD053", "link-image-reference-definitions", "Link and image reference definitions should be needed");

        public static RuleInfo MD054 = new("MD054", "link-image-style", "Link and image style");

        public static RuleInfo MD061 = new("MD061", "file-links-exist", "File links should reference existing files");

        public static RuleInfo MD062 = new("MD062", "image-links-exist", "Image links should reference existing files");

        // ===== Inline Rules =====

        public static RuleInfo MD033 = new("MD033", "no-inline-html", "Inline HTML");

        public static RuleInfo MD035 = new("MD035", "hr-style", "Horizontal rule style");

        public static RuleInfo MD036 = new("MD036", "no-emphasis-as-heading", "Emphasis used instead of a heading");

        public static RuleInfo MD037 = new("MD037", "no-space-in-emphasis", "Spaces inside emphasis markers");

        public static RuleInfo MD038 = new("MD038", "no-space-in-code", "Spaces inside code span elements");

        public static RuleInfo MD049 = new("MD049", "emphasis-style", "Emphasis style should be consistent");

        public static RuleInfo MD050 = new("MD050", "strong-style", "Strong style should be consistent");

        // ===== Blockquote Rules =====

        public static RuleInfo MD027 = new("MD027", "no-multiple-space-blockquote", "Multiple spaces after blockquote symbol");

        public static RuleInfo MD028 = new("MD028", "no-blanks-blockquote", "Blank line inside blockquote");

        // ===== Table Rules =====

        public static RuleInfo MD055 = new("MD055", "table-pipe-style", "Table pipe style");

        public static RuleInfo MD056 = new("MD056", "table-column-count", "Table column count");

        public static RuleInfo MD058 = new("MD058", "blanks-around-tables", "Tables should be surrounded by blank lines");

        public static RuleInfo MD060 = new("MD060", "table-column-style", "Table column style should be consistent");

        // ===== Accessibility Rules =====

        public static RuleInfo MD059 = new("MD059", "descriptive-link-text", "Link text should be descriptive");
    }
}
