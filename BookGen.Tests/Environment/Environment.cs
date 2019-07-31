using System;
using System.IO;

namespace BookGen.Tests.Environment
{
    public static class TestEnvironment
    {
        public static string GetFile(string file)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Environment", file);
        }
    }
}
