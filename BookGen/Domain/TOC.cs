//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Contracts;
using System.Collections.Generic;

namespace BookGen.Domain
{
    public class ToC : IToC
    {
        private readonly Dictionary<string, List<HtmlLink>> _tocContents;

        public int ChapterCount
        {
            get { return _tocContents.Keys.Count; }
        }

        public int FilesCount
        {
            get
            {
                int count = 0;
                foreach (var chapter in _tocContents.Keys)
                {
                    count += _tocContents[chapter].Count;
                }
                return count;
            }
        }


        public ToC()
        {
            _tocContents = new Dictionary<string, List<HtmlLink>>();
        }

        public void AddChapter(string chapter, List<HtmlLink> files)
        {
            _tocContents.Add(chapter, files);
        }

        public IEnumerable<string> Chapters
        {
            get { return _tocContents.Keys; }
        }

        public IEnumerable<HtmlLink> GetLinksForChapter(string? chapter = null)
        {
            if (string.IsNullOrEmpty(chapter))
            {
                List<HtmlLink> merged = new List<HtmlLink>();
                foreach (var v in _tocContents.Values)
                {
                    merged.AddRange(v);
                }
                return merged;
            }
            else
                return _tocContents[chapter];
        }

        public IEnumerable<string> Files
        {
            get
            {
                foreach (var chapter in _tocContents.Keys)
                {
                    foreach (var file in _tocContents[chapter])
                    {
                        yield return file.Link;
                    }
                }
            }
        }
    }
}
