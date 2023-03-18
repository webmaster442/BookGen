//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

public sealed class TagsArguments : BookGenArgumentBase
{
    [Switch("a", "auto")]
    public bool AutoGenerateTags { get; set; }

    [Switch("k", "keywordcount")]
    public int AutoKeyWordCount { get; set; }

    public TagsArguments()
    {
        AutoGenerateTags = false;
        AutoKeyWordCount = 10;
    }
}
