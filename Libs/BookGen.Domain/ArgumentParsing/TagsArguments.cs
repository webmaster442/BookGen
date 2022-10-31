//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain.ArgumentParsing
{
    public sealed class TagsArguments : BookGenArgumentBase
    {
        [Switch("a", "auto", false)]
        public bool AutoGenerateTags { get; set; }

        [Switch("k", "keywordcount")]
        public int AutoKeyWordCount { get; set; }

        public TagsArguments()
        {
            AutoGenerateTags = false;
            AutoKeyWordCount = 10;
        }
    }
}
