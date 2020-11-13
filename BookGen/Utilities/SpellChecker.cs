//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Core;
using Markdig;
using Markdig.Extensions.AutoIdentifiers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeCantSpell.Hunspell;

namespace BookGen.Utilities
{
    public class SpellChecker
    {
        private readonly ILog _log;
        private readonly IAppSetting _appSettings;
        private WordList? _wordList;

        public SpellChecker(ILog log, IAppSetting appSettings, string language)
        {
            _log = log;
            _appSettings = appSettings;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ConfigureWordList(log, language);
        }

        private void ConfigureWordList(ILog log, string spellLanguage)
        {
            if (string.IsNullOrEmpty(spellLanguage))
            {
                _log.Warning("No spell language set. Please set one using Application settings");
                return;
            }

            var baseDir = new FsPath(_appSettings.AppDataPath);
            var aff = baseDir.Combine($"{spellLanguage}.aff");
            var dic = baseDir.Combine($"{spellLanguage}.dic");

            if (aff.IsExisting &&
                dic.IsExisting)
            {
                _wordList = WordList.CreateFromFiles(dic.ToString(), aff.ToString());
            }
            else
            {
                _log.Warning("Can't find dictionary for {0} language. Try installing it.", spellLanguage);
            }

        }

        public void SpellCheck(FsPath file)
        {
            if (_wordList == null) return;

            if (!file.IsExisting)
            {
                _log.Warning("File not found: {0}", file);
                return;
            }

            string content = file.ReadFile(_log);

            var pipeline = new MarkdownPipelineBuilder().UseAutoIdentifiers(AutoIdentifierOptions.GitHub).Build();
            var doc = Markdown.Parse(content, pipeline);

            StringBuilder results = new StringBuilder();

            foreach (MarkdownObject item in doc.Descendants())
            {
                if (item is CodeBlock) continue; //skip code

                if (item is Inline inline)
                {
                    var blockContent = GetWords(inline.ToString());

                    if (blockContent == null) return;

                    int i = 0;
                    foreach (var word in blockContent)
                    {
                        if (!_wordList.Check(word))
                        {
                            var suggestions = _wordList.Suggest(word).Take(10);
                            PrintSuggestions(item.Line, blockContent, i, suggestions);
                        }
                        ++i;
                    }

                }
            }
        }

        private string[] GetWords(string? v)
        {
            if (v == null) 
                return Enumerable.Empty<string>().ToArray();

            return v.Split(new char[] { ' ', ',', '.', '?', ':', '"', '"', '!', '(', ')', '^', '[', ']', '-' },
                           System.StringSplitOptions.RemoveEmptyEntries);
        }

        private void PrintSuggestions(int line, string[] blockContent, int index, IEnumerable<string> suggestions)
        {
            const int wordsInContext = 1;
            int count = (index + wordsInContext) - (index - wordsInContext);

            string? context = null;
            if ((index - wordsInContext) >= 0
                && (index + wordsInContext) < blockContent.Length - 1)
            {
                context = string.Join(' ', blockContent, index - wordsInContext, count);
            }
            else if ((index + wordsInContext) < blockContent.Length - 1)
            {
                context = string.Join(' ', blockContent, 0, count);
            }
            else
            {
                context = blockContent[index];
            }

            _log.Warning("line {0} {1}: ...\"{2}\"...", line, blockContent[index], context);
            _log.Warning(string.Join("\r\n", suggestions));
        }
    }
}
