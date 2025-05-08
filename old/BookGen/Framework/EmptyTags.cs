//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Framework;

internal sealed class EmptyTagUtils : ITagUtils
{
    public int UniqueTagCount => 0;
    public int TotalTagCount => 0;
    public int FilesWithOutTags => 0;

    public ISet<string> GetTagsForFile(string file)
        => new HashSet<string>();

    public ISet<string> GetTagsForFiles(IEnumerable<string> files)
        => new HashSet<string>();

    public string GetUrlNiceName(string tag)
        => string.Empty;
}
