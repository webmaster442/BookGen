//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Configuration;
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

        public static string GetTestFolder()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Environment");
        }

        public static ILog GetMockedLog()
        {
            var mock = new Mock<ILog>();

            return mock.Object;
        }

        public static IReadonlyRuntimeSettings GetMockedSettings()
        {
            var testConfig = Config.CreateDefault();
            testConfig.HostName = "http://test.com/";

            var mock = new Mock<IReadonlyRuntimeSettings>();
            mock.SetupGet(m => m.Configuration).Returns(testConfig);
            mock.SetupGet(m => m.OutputDirectory).Returns(new FsPath(GetTestFolder()));

            return mock.Object;
        }
    }
}
