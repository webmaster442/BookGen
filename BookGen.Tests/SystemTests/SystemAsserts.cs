//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace BookGen.Tests.SystemTests
{
    internal static class SystemAsserts
    {
        public static void FileExists(string path)
        {
            if (!File.Exists(path))
                Assert.Fail("File doesn't exist: {0}", path);
        }

        public static void FileHasContent(string path)
        {
            FileInfo fi = new FileInfo(path);
            if (fi.Length < 1)
                Assert.Fail("File is empty (0 bytes): {0}", path);
        }

        public static void FileContainsStrings(string path, IEnumerable<string> strings)
        {
            string contents = File.ReadAllText(path);
            foreach (var str in strings)
            {
                if (!contents.Contains(str))
                    Assert.Fail("{0} is not found in file: {1}", str, path);
            }
        }

        public static void FileNotConainsStrings(string path, IEnumerable<string> strings)
        {
            string contents = File.ReadAllText(path);
            foreach (var str in strings)
            {
                if (contents.Contains(str))
                    Assert.Fail("{0} is found in file: {1}", str, path);
            }
        }
    }
}
