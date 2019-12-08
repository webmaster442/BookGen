//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using WeCantSpell.Hunspell;

namespace BookGen.Editor.Controls
{
    public sealed class Hunspell
    {
        private readonly WordList wordList;

        public Hunspell(string aff, string dict)
        {
            wordList = WordList.CreateFromFiles(dict, aff);
        }

        public bool IsSpelledCorrectly(string word)
        {
            return wordList.Check(word);
        }

        public List<string> GetSuggestions(string word)
        {
            return wordList.Suggest(word).Take(15).ToList();
        }
    }
}
