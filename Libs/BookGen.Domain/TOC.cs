﻿//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections;

using BookGen.Interfaces;

namespace BookGen.Domain
{
    public sealed class ToC : ITableOfContents
    {
        private readonly Dictionary<string, List<Link>> _tocContents;

        public int ChapterCount
        {
            get { return _tocContents.Keys.Count; }
        }

        public int FilesCount
        {
            get
            {
                int count = 0;
                foreach (string? chapter in _tocContents.Keys)
                {
                    count += _tocContents[chapter].Count;
                }
                return count;
            }
        }

        public ToC()
        {
            _tocContents = [];
            RawMarkdown = string.Empty;
        }

        public void AddChapter(string chapter, List<Link> files)
        {
            _tocContents.Add(chapter, files);
        }

        public IEnumerable<string> Chapters
        {
            get { return _tocContents.Keys; }
        }

        public IEnumerable<Link> GetLinksForChapter(string? chapter = null)
        {
            if (string.IsNullOrEmpty(chapter))
            {
                var merged = new List<Link>();
                foreach (List<Link>? v in _tocContents.Values)
                {
                    merged.AddRange(v);
                }
                return merged;
            }
            else
            {
                return _tocContents[chapter];
            }
        }

        public IEnumerator<Link> GetEnumerator()
        {
            return _tocContents
                .SelectMany(x => x.Value)
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<string> Files
        {
            get
            {
                foreach (string? chapter in _tocContents.Keys)
                {
                    foreach (Link? file in _tocContents[chapter])
                    {
                        yield return file.Url;
                    }
                }
            }
        }

        public string RawMarkdown { get; set; }
    }
}
