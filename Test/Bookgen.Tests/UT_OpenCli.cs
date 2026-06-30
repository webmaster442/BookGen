//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Domain.IO.Legacy;

using BookGen.Cli;
using BookGen.Cli.OpenCli.Draft;
using BookGen.Commands;
using BookGen.Infrastructure;

using DocumentFormat.OpenXml.Office2016.Excel;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using Moq;

namespace Bookgen.Tests;

[TestFixture]
internal class UT_OpenCli
{
    private Document _openCliDocument;
    private List<string> _globalOptions;

    private static CommandRunner SetupCommandRunner()
    {
        var helpProviderMock = new Mock<ICommandHelpProvider>(MockBehavior.Strict);
        var loggerMock = new Mock<ILogger>(MockBehavior.Strict);
        var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);
        var runner = new CommandRunner(serviceProviderMock.Object, helpProviderMock.Object, loggerMock.Object, CommandRunnerSettings.Default);
        runner.AddCommandsFrom(typeof(HelpCommand).Assembly);
        return runner;
    }

    [SetUp]
    public void Setup()
    {
        var runner = SetupCommandRunner();
        _openCliDocument = runner.GenerateOpenCliDocs();
        _globalOptions = runner.GetGlobalOptions().ToList();
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
            Assert.That(_openCliDocument.Info.Title, Is.Not.Empty);
            Assert.That(_openCliDocument.Info.Version, Is.Not.Empty);
        }
    }

    [Test]
    public void EnsureThat_OpenCli_Documentation_ContainsGlobalOptions()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_openCliDocument.Command.Options, Is.Not.Empty);
        }
    }

    [TestCaseSource(nameof(GlobalOptions))]
    public void EnsureThat_OpenCli_Documentation_ContainsGlobalOptions(string globalOption)
    {
        var option = _openCliDocument.Command.Options?.Where(o => o.Name == globalOption).FirstOrDefault();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(option, Is.Not.Null, $"Global option '{globalOption}' is missing in OpenCli documentation.");
            Assert.That(option?.Description, Is.Not.Null.And.Not.Empty, $"Global option '{globalOption}' is missing description in OpenCli documentation.");
        }
    }


    [TestCaseSource(nameof(Commands))]
    public void EnsureThat_OpenCli_Documentation_Is_Filled_Out(string command)
    {

    }
}
