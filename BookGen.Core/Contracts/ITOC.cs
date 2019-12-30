//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Core.Contracts
{
    public interface IToC
    {
        void AddChapter(string chapter, List<HtmlLink> files);
        IEnumerable<string> Chapters { get; }
        IEnumerable<HtmlLink> GetLinksForChapter(string? chapter = null);
        IEnumerable<string> Files { get; }
    }
}
