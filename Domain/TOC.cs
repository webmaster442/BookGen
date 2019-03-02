//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;

namespace BookGen.Domain
{
    public class TOC: IEnumerable<(string chapters, List<string> files)>
    {
        private readonly Dictionary<string, List<string>> _tocContents;

        public TOC()
        {
            _tocContents = new Dictionary<string, List<string>>();
        }

        public void AddChapter(string chapter, List<string> files)
        {
            _tocContents.Add(chapter, files);
        }

        public IEnumerable<string> Files
        {
            get
            {
                foreach (var chapter in _tocContents.Keys)
                {
                    foreach (var file in _tocContents[chapter])
                    {
                        yield return file;
                    }
                }
            }
        }

        public IEnumerator<(string chapters, List<string> files)> GetEnumerator()
        {
            foreach (var item in _tocContents)
            {
                yield return (item.Key, item.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
