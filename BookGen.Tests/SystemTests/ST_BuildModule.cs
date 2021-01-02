using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
