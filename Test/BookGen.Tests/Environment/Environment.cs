//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
            var mock = Substitute.For<IAppSetting>();
            mock.NodeJsPath.Returns(string.Empty);
            mock.NodeJsTimeout.Returns(60);
            return mock;
        }

        public static ILog GetMockedLog()
        {
            var mock = Substitute.For<ILog>();
            return mock;
        }

        public static IReadonlyRuntimeSettings GetMockedSettings()
        {
            var testConfig = Config.CreateDefault();
            testConfig.HostName = "http://test.com/";

            var mock = Substitute.For<IReadonlyRuntimeSettings>();
            mock.Configuration.Returns(testConfig);
            mock.OutputDirectory.Returns(new FsPath(GetTestFolder()));
            mock.SourceDirectory.Returns(new FsPath(GetTestFolder()));
            return mock;
        }
    }
}
