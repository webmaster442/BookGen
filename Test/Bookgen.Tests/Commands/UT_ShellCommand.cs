using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BookGen;
using BookGen.Cli;
using BookGen.Commands;

using Moq;

namespace Bookgen.Tests.Commands;


[TestFixture]
internal class UT_ShellCommand : CommandTestBase<ShellCommand>
{
    protected override ShellCommand CreateSut()
    {
        return new ShellCommand(CommandRunnerProxyMock.Object);
    }

    protected override void SetupMocks()
    {
        CommandRunnerProxyMock.Setup(x => x.CommandNames).Returns(new string[] { "validate", "shell", "gui", "addfrontmatter", "check" });
    }

    [Test]
    public async Task Test_Execute_NoArgs()
    {
        var result = await Command.ExecuteAsync(ArgumentsBase.Empty, ["c"]);
        Assert.That(result, Is.EqualTo(ExitCodes.Succes));
        CommandRunnerProxyMock.Verify(x => x.CommandNames, Times.Once);
    }
}
