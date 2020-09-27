//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Domain;
using System.Collections.Generic;
using System.IO;

namespace BookGen.Utilities
{
    public static class ChapterSerialer
    {
        public static void WriteToFile(FsPath target, IEnumerable<Chapter> chapters)
        {
            using (var writer = File.CreateText(target.ToString()))
            {
                foreach (var chapter in chapters)
                {
                    writer.WriteLine(chapter.Title);
                    foreach (var file in chapter.Files)
                    {
                        writer.WriteLine($"\t{file}");
                    }
                    writer.WriteLine();
                }
            }
        }

        public static IEnumerable<Chapter> ReadFromFile(FsPath source)
        {
            List<string> files = new List<string>();
            using (var reader = File.OpenText(source.ToString()))
            {
                int counter = 0;
                string? line;
                do
                {
                    line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line) || line.StartsWith('#')) continue;
                    if (StartsWithWhiteSpace(line))
                    {
                        files.Add(line.Trim());
                    }
                    else
                    {
                        string title = line.Trim();
                        ++counter;
                        if (counter > 0)
                        {
                            Chapter ret = new Chapter
                            {
                                Title = title,
                                Files = new List<string>(files)
                            };
                            files.Clear();
                            yield return ret;
                        }
                    }


                }
                while (line != null);
            }
        }

        private static bool StartsWithWhiteSpace(string line)
        {
            return line.StartsWith('\t') || line.StartsWith(' ');
        }
    }
}
