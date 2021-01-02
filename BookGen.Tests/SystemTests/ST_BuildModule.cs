//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using NUnit.Framework;

namespace BookGen.Tests.SystemTests
{
    [TestFixture]
    public class ST_BuildModule : SystemTestBase
    {
        [Test]
        public void TestHtmlBuild()
        {
            CopyDemoProject();
            CreateConfigFile();
            RunProgram("Build", "-n", "-d", Workdir, "-a", "BuildWeb");
        }
    }
}
