//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using NUnit.Framework;
using System.IO;

namespace BookGen.Tests.SystemTests
{
    internal static class SystemAsserts
    {
        public static void FileExists(params string[] parts)
        {
            string fullPath = Path.Combine(parts);
            if (!File.Exists(fullPath))
                Assert.Fail("File doesn't exist: {0}", fullPath);
        }

        public static void FileHasContent(params string[] parts)
        {
            string fullPath = Path.Combine(parts);
            FileInfo fi = new FileInfo(fullPath);
            if (fi.Length < 1)
                Assert.Fail("File is empty (0 bytes): {0}", fullPath);
        }
    }
}
