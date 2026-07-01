//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.OpenCli.Draft;
using BookGen.Commands;
using BookGen.GlobalOptionParsers;

using Microsoft.Extensions.Logging;

using Moq;

namespace Bookgen.Tests;

[TestFixture]
internal class UT_OpenCli
{
    private Document _openCliDocument;

    private static CommandRunner SetupCommandRunner()
    {
        var helpProviderMock = new Mock<ICommandHelpProvider>(MockBehavior.Strict);
        var loggerMock = new Mock<ILogger>(MockBehavior.Strict);
        var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);
        var runner = new CommandRunner(serviceProviderMock.Object, helpProviderMock.Object, loggerMock.Object, CommandRunnerSettings.Default);

        var info = new BookGen.ProgramInfo();

        runner
            .AddGlobalOptionParser<AttachDebuggerParser>()
            .AddGlobalOptionParser<WaitDebuggerParser>()
            .AddGlobalOptionParser(new JsonLogParser(info))
            .AddGlobalOptionParser(new LogToFileParser(info))
            .AddGlobalOptionParser(new RuntimePrintingParser(info));

        runner.AddDefaultCommand<HelpCommand>();
        runner.AddCommandsFrom(typeof(HelpCommand).Assembly);
        return runner;
    }

    [SetUp]
    public void Setup()
    {
        var runner = SetupCommandRunner();
        _openCliDocument = runner.GenerateOpenCliDocs();
    }

    public static IEnumerable<string> Commands
    {
        get
        {
            var runner = SetupCommandRunner();
            return runner.CommandNames;
        }
    }

    public static IEnumerable<string> GlobalOptions
    {
        get
        {
            var runner = SetupCommandRunner();
            return runner.GetGlobalOptions();
        }
    }

    [Test]
    public void EnsureThat_OpenCli_Documentation_Structure_Ok()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_openCliDocument.Commands, Is.Not.Empty);
            Assert.That(_openCliDocument.Opencli, Is.EqualTo("0.1"));
            Assert.That(_openCliDocument.Info.Version, Is.Not.Empty);
        }
    }

    [Test]
    public void EnsureThat_OpenCli_Documentation_GlobalOptions_NotEmpty()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_openCliDocument.Command.Options, Is.Not.Empty);
        }
    }

    [TestCaseSource(nameof(GlobalOptions))]
    public void EnsureThat_OpenCli_Documentation_GlobalOption_CorrectlyDocumented(string globalOption)
    {
        Option? option = _openCliDocument.Command.Options?.Where(o => o.Name == globalOption || o.Aliases?.Contains(globalOption) == true).FirstOrDefault();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(option, Is.Not.Null, $"Global option '{globalOption}' is missing in OpenCli documentation.");
            Assert.That(option?.Description, Is.Not.Null.And.Not.Empty, $"Global option '{globalOption}' is missing description in OpenCli documentation.");
            Assert.That(option?.OpenClRequired, Is.EqualTo(false), $"Global option '{globalOption}' should have OpenClRequired set to false in OpenCli documentation.");
            Assert.That(option?.Aliases, Is.Not.Null, $"Global option '{globalOption}' is missing aliases in OpenCli documentation.");
            Assert.That(option?.Name, Is.Not.Null.And.Not.Empty, $"Global option '{globalOption}' is missing name in OpenCli documentation.");
        }
    }


    [TestCaseSource(nameof(Commands))]
    public void EnsureThat_OpenCli_Documentation_Command_CorrectlyDocumented(string command)
    {
        BookGen.Cli.OpenCli.Draft.Command? cmd = _openCliDocument.Commands?.Where(c => c.Name == command).FirstOrDefault();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(cmd, Is.Not.Null, $"Command '{command}' is missing in OpenCli documentation.");
            Assert.That(cmd?.Description, Is.Not.Null.And.Not.Empty, $"Command '{command}' is missing description in OpenCli documentation.");
            Assert.That(cmd?.Name, Is.Not.Null.And.Not.Empty, $"Command '{command}' is missing name in OpenCli documentation.");

            Assert.That(cmd?.ExitCodes, Has.Count.GreaterThanOrEqualTo(1), $"Command '{command}' is missing exit codes in OpenCli documentation.");

            foreach (ExitCode exitCode in cmd?.ExitCodes ?? Enumerable.Empty<ExitCode>())
            {
                Assert.That(exitCode.Description, Is.Not.Null.And.Not.Empty, $"Command '{command}' has an exit code with null or empty description in OpenCli documentation.");
            }

            if (cmd?.Options != null)
            {
                foreach (Option option in cmd.Options)
                {
                    Assert.That(option.Description, Is.Not.Null.And.Not.Empty, $"Option '{option.Name}' of command '{command}' is missing description in OpenCli documentation.");
                    Assert.That(option.Name, Is.Not.Null.And.Not.Empty, $"Option of command '{command}' is missing name in OpenCli documentation.");
                    Assert.That(option.OpenClRequired, Is.Not.Null, $"Option '{option.Name}' of command '{command}' is missing OpenClRequired in OpenCli documentation.");
                }
            }

            if (cmd?.Arguments != null)
            {
                foreach (Argument argument in cmd.Arguments)
                {
                    Assert.That(argument.Description, Is.Not.Null.And.Not.Empty, $"Argument '{argument.Name}' of command '{command}' is missing description in OpenCli documentation.");
                    Assert.That(argument.Name, Is.Not.Null.And.Not.Empty, $"Argument of command '{command}' is missing name in OpenCli documentation.");
                    Assert.That(argument.OpenClRequired, Is.Not.Null, $"Argument '{argument.Name}' of command '{command}' is missing OpenClRequired in OpenCli documentation.");
                }

            }
        }
    }
}
