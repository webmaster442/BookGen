//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Core.Contracts
{
    public interface ITOC
    {
        void AddChapter(string chapter, List<HtmlLink> files);
        IEnumerable<string> Chapters { get; }
        IEnumerable<string> GetFilesForChapter(string chapter);
        IEnumerable<HtmlLink> GetLinksForChapter(string chapter);
        IEnumerable<string> Files { get; }
    }
}
