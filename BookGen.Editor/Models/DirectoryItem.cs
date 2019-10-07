//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Editor.Models
{
    public class DirectoryItem
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public int FileCount { get; set; }
        public IList<DirectoryItem> SubDirs { get; set; }
    }
}
