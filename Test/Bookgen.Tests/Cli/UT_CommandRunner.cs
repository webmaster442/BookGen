using BookGen.Cli;
using BookGen.Cli.Annotations;

using Microsoft.Extensions.Logging;

using Moq;

namespace Bookgen.Tests.Cli;

[TestFixture]
internal class UT_CommandRunner
{
    private CommandRunner _sut;
    private Mock<IResolver> _resolverMock;
    private Mock<ILogger> _loggerMock;

    private sealed class Dependency
    {
        public int Value => 5;
    }

    [CommandName("test")]
    private sealed class TestCommand : Command<TestCommand.Settings>
    {
        public Dependency Dependency { get; }

        public class Settings : ArgumentsBase
        {
            [Switch("v", "value")]
            public int Value { get; set; }
        }

        public TestCommand(Dependency spy)
        {
            Dependency = spy;
        }

        public override int Execute(Settings arguments, IReadOnlyList<string> context)
        {
            return arguments.Value * Dependency.Value;
        }
    }

    [SetUp]
    public void Setup()
    {
        _resolverMock = new Mock<IResolver>(MockBehavior.Strict);
        _resolverMock.Setup(x => x.Resolve(typeof(Dependency), It.IsAny<string>())).Returns(new Dependency());
        _loggerMock = new Mock<ILogger>(MockBehavior.Strict);
        _sut = new CommandRunner(_resolverMock.Object, _loggerMock.Object, new CommandRunnerSettings
        {
           
        });
        _sut.AddCommand<TestCommand>();
    }

    [Test]
    public async Task EnsureThat_Run_Works()
    {
        string[] args = ["test", "-v", "2"];
        int result = await _sut.Run(args);
        Assert.That(result, Is.EqualTo(10));
    }
}
