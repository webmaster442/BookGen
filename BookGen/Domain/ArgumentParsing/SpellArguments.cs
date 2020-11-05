//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Ui.ArgumentParser;

namespace BookGen.Domain.ArgumentParsing
{
    internal class SpellArguments : ArgumentsBase
    {
        [Switch("l", "language", false)]
        public string LanguageCode { get; set; }

        [Switch("a", "action", true)]
        public SpellActions Action { get; set; }

        public SpellArguments()
        {
            LanguageCode = string.Empty;
        }

        public override bool Validate()
        {
            if (Action == SpellActions.Check
                && Files.Length == 1)
            {
                return !string.IsNullOrEmpty(LanguageCode);
            }
            else if (Action == SpellActions.Install
                && Files.Length == 0)
            {
                return !string.IsNullOrEmpty(LanguageCode);
            }
            else
            {
                return Files.Length == 0;
            }
        }
    }
}
