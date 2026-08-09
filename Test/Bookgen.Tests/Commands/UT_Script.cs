using System.IO.Pipes;

using BookGen;
using BookGen.Commands;

using Microsoft.Extensions.Logging;

using Moq;

namespace Bookgen.Tests.Commands;

[TestFixture]
internal class UT_Script : CommandTestBase<ScriptCommand>
{
    private string _samplescript = null!;
    private string _multilineScript = null!;

    protected override void SetupMocks()
    {
        _samplescript = """
            #log executing cmd1
            cmd1 arg1 arg2 #log this shouldn't be logged
            #log done executing
            convert qrcode -d https://example.com/page#section
            #regular coment
            """;

        _multilineScript = """
            cmd1 # note \
            cmd2
            """;

        CommandRunnerProxyMock.Setup(c => c.CommandNames).Returns(["cmd1", "cmd2", "convert qrcode"]);
        CommandRunnerProxyMock.Setup(x => x.RunCommandAsync("cmd1", It.IsAny<string[]>())).Returns(Task.FromResult(0));
        CommandRunnerProxyMock.Setup(x => x.RunCommandAsync("cmd2", It.IsAny<string[]>())).Returns(Task.FromResult(0));
        CommandRunnerProxyMock.Setup(x => x.RunCommandAsync("convert qrcode", new string[] { "-d", "https://example.com/page#section" })).Returns(Task.FromResult(0));

        FileSystemMock.Setup(x => x.FileExists("sample.script")).Returns(true);
        FileSystemMock.Setup(x => x.FileExists("multi.script")).Returns(true);
        FileSystemMock.Setup(x => x.OpenTextReader("sample.script")).Returns(new StringReader(_samplescript));
        FileSystemMock.Setup(x => x.OpenTextReader("multi.script")).Returns(new StringReader(_multilineScript));
        LoggerMock.Setup(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()));
    }

    protected override ScriptCommand CreateSut()
    {
        return new ScriptCommand(CommandRunnerProxyMock.Object,
                                 LoggerMock.Object,
                                 FileSystemMock.Object);
    }

    [Test]
    [CancelAfter(TenSeconds)]
    public async Task EnsureThat_SampleScript_Runs(CancellationToken token)
    {
        var args = new ScriptCommand.Arguments { ScriptPath = "sample.script" };
        int exitCode = await Command.ExecuteAsync(args, Array.Empty<string>(), token);

        using (Assert.EnterMultipleScope())
        {
            Assert.AreEqual(exitCode, ExitCodes.Success);
            CommandRunnerProxyMock.Verify(x => x.RunCommandAsync("cmd1", new string[] { "arg1", "arg2" }), Times.Once);
            CommandRunnerProxyMock.Verify(x => x.RunCommandAsync("convert qrcode", new string[] { "-d", "https://example.com/page#section" }), Times.Once);
            LoggerMock.Verify(x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("executing cmd1")), null, It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
            LoggerMock.Verify(x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("done executing")), null, It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        }
    }

    [Test]
    [CancelAfter(TenSeconds)]
    public async Task EnsureThat_MultiLineCommandsWork(CancellationToken token)
    {
        var args = new ScriptCommand.Arguments { ScriptPath = "multi.script" };
        int exitCode = await Command.ExecuteAsync(args, Array.Empty<string>(), token);

        using (Assert.EnterMultipleScope())
        {
            Assert.AreEqual(exitCode, ExitCodes.Success);
            CommandRunnerProxyMock.Verify(x => x.RunCommandAsync("cmd1", It.IsAny<string[]>()), Times.Once);
            CommandRunnerProxyMock.Verify(x => x.RunCommandAsync("cmd2", It.IsAny<string[]>()), Times.Once);
        }
    }
}
