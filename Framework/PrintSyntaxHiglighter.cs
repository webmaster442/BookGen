//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BookGen.Framework
{
    internal class PrintSyntaxHiglighter : HtmlObjectRenderer<CodeBlock>
    {
        private readonly CodeBlockRenderer _underlyingRenderer;
        private Dictionary<string, Regex> _keywordsCache;

        public PrintSyntaxHiglighter(CodeBlockRenderer underlyingRenderer)
        {
            _underlyingRenderer = underlyingRenderer ?? new CodeBlockRenderer();
        }

        private Regex GetReplaceRegex(string language)
        {
            if (_keywordsCache == null)
                _keywordsCache = new Dictionary<string, Regex>();

            if (_keywordsCache.ContainsKey(language))
                return _keywordsCache[language];

            switch (language)
            {
                case "csharp":
                    _keywordsCache.Add(language, BuildRegex(Properties.Resources.keywords_csharp));
                    return _keywordsCache[language];
                case "bash":
                    _keywordsCache.Add(language, BuildRegex(Properties.Resources.keywords_bash));
                    return _keywordsCache[language];
                default:
                    return null;
            }
        }

        private Regex BuildRegex(string keywords)
        {
            var pattern = new StringBuilder();
            pattern.Append("\\b(");
            foreach (var word in keywords.Split('\n'))
            {
                pattern.Append(word);
                pattern.Append("|");
            }
            pattern[pattern.Length - 1] = ')';
            pattern.Append("\\b");

            return new Regex(pattern.ToString(), RegexOptions.Compiled);
        }

        private static string GetCode(LeafBlock obj, out string firstLine)
        {
            var code = new StringBuilder();
            firstLine = null;
            foreach (var line in obj.Lines.Lines)
            {
                var slice = line.Slice;
                if (slice.Text == null)
                    continue;

                var lineText = slice.Text.Substring(slice.Start, slice.Length);

                if (firstLine == null)
                    firstLine = lineText;
                else
                    code.AppendLine();

                code.Append(lineText);
            }
            return code.ToString();
        }

        protected override void Write(HtmlRenderer renderer, CodeBlock obj)
        {
            var parser = obj.Parser as FencedCodeBlockParser;
            if (!(obj is FencedCodeBlock fencedCodeBlock) || parser == null)
            {
                _underlyingRenderer.Write(renderer, obj);
                return;
            }

            var languageMoniker = fencedCodeBlock.Info.Replace(parser.InfoPrefix, string.Empty);
            if (string.IsNullOrEmpty(languageMoniker))
            {
                _underlyingRenderer.Write(renderer, obj);
                return;
            }

            string firstLine;
            var code = GetCode(obj, out firstLine);

            var rendered = Render(code, languageMoniker);

            renderer.Write(rendered);
        }

        private string Render(string code, string languageMoniker)
        {
            var pattrern = GetReplaceRegex(languageMoniker);

            if (pattrern == null)
            {
                return code;
            }

            StringBuilder output = new StringBuilder();
            string[] lines = code.Split('\n');
            string format = "{0:D" + lines.Length.ToString().Length.ToString() + "}: ";
            output.AppendLine("<pre>");
            int num = 1;
            foreach (string input in lines)
            {
                output.AppendFormat(format, num);
                string s = HTML(input, pattrern);
                output.Append(s);
                output.Append("\n");
                num++;
            }
            output.AppendLine("</pre>");
            return output.ToString();
        }

        private string HTML(string input, Regex pattrern)
        {
            StringBuilder replacer = new StringBuilder(input);
            replacer.Replace("<", "&#60;");
            replacer.Replace(">", "&#62;");
            replacer.Replace("    ", "  ");
            return pattrern.Replace(replacer.ToString(), "<b>$&</b>");
        }
    }
}
