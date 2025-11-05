using System.Net.WebSockets;
using System.Text;

using BookGen.Cli;
using BookGen.Commands;
using BookGen.Infrastructure;

using Microsoft.Extensions.Logging;

using Moq;

namespace Bookgen.Tests;

[TestFixture]
internal class TestDocumentation
{
    private Mock<IServiceProvider> _serviceProviderMock;
    private Mock<ILogger> _loggerMock;
    private HelpProvider _helpProvider;
    private CommandRunnerProxy _commandRunnerProxy;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger>(MockBehavior.Strict);
        _serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);
        var testCommandRunner = new CommandRunner(_serviceProviderMock.Object, _loggerMock.Object, new CommandRunnerSettings());
        testCommandRunner.AddCommandsFrom(typeof(HelpCommand).Assembly);

        _commandRunnerProxy = new();
        _commandRunnerProxy.ConfigureWith(testCommandRunner);

        _helpProvider = new HelpProvider(_loggerMock.Object, _commandRunnerProxy);
    }
    
    [Test]
    public void EnsureThat_Commands_HaveDocumentation()
    {
        StringBuilder errorMessage = new();
        var hasHelp = _helpProvider.HelpEntries.ToHashSet();
        foreach (var commandName in _commandRunnerProxy.CommandNames)
        {
            if (!hasHelp.Contains(commandName))
            {
                errorMessage.AppendLine($"No help was found for command: {commandName}");
            }
        }
        if (errorMessage.Length > 0)
            Assert.Fail(errorMessage.ToString());
    }

    [Test]
    public void EnsureThat_Documentation_ContainsOnlyExistingCommands()
    {
        StringBuilder errorMessage = new();
        var commands = _commandRunnerProxy.CommandNames.ToHashSet();
        foreach (var helpEntry in _helpProvider.HelpEntries)
        {
            if (!commands.Contains(helpEntry))
            {
                errorMessage.AppendLine($"No command found for documentation: {helpEntry}");
            }
        }
        if (errorMessage.Length > 0)
            Assert.Fail(errorMessage.ToString());
    }
}
