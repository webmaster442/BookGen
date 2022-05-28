//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Core.Contracts
{
    public interface ITagUtils
    {
        ISet<string> TagsForFile(string file);
        ISet<string> TagsForFiles(IEnumerable<string> files);
    }
}
