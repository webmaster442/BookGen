﻿//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Api
{
    public interface ITableOfContents
    {
        /// <summary>
        /// A flat list of chapters without hierarchy
        /// </summary>
        IEnumerable<string> Chapters { get; }

        /// <summary>
        /// All files referenced in the Table of Contents
        /// </summary>
        IEnumerable<string> Files { get; }

        /// <summary>
        /// Gets Links for a chapter
        /// </summary>
        /// <param name="chapter">chapter name. Can be null. If null, all links returned from the TOC</param>
        /// <returns>Enumerable collection of HTMLLinks</returns>
        IEnumerable<Link> GetLinksForChapter(string? chapter = null);
    }
}
