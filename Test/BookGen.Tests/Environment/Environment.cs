//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Domain.Configuration;
using BookGen.Interfaces;
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

        public static string GetSystemTestContentFolder()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Environment", "SystemTest");
        }

        internal static IAppSetting GetMockedAppSettings()
        {
            var mock = new Mock<IAppSetting>();
            mock.SetupGet(x => x.NodeJsPath).Returns("");
            mock.SetupGet(x => x.NodeJsTimeout).Returns(60);

            return mock.Object;
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
            mock.SetupGet(m => m.SourceDirectory).Returns(new FsPath(GetTestFolder()));

            return mock.Object;
        }
    }
}
