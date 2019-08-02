//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using Moq;
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

        public static Mock<ILog> GetMockedLog()
        {
            return new Mock<ILog>();
        }

        public static Mock<IReadonlyRuntimeSettings> GetMockedSettings()
        {
            return new Mock<IReadonlyRuntimeSettings>();
        }
    }
}
