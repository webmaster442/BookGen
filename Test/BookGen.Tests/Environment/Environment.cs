//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.IO;

using Microsoft.Extensions.Logging;

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

        public static ILogger GetMockedLog()
        {
            var mock = Substitute.For<ILogger>();
            return mock;
        }

        public static IReadonlyRuntimeSettings GetMockedRuntimeSettings()
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
