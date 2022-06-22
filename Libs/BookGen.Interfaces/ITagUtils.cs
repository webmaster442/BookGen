//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using System.Collections.Generic;

namespace BookGen.Interfaces
{
    public interface ITagUtils
    {
        ISet<string> GetTagsForFile(string file);
        ISet<string> GetTagsForFiles(IEnumerable<string> files);

        string GetUrlNiceName(string tag);

        int UniqueTagCount { get; }
        int TotalTagCount { get; }
        int FilesWithOutTags { get; }
    }
}
