using Bookgen.Lib.VFS;

using BookGen.Cli;

using Microsoft.Extensions.Logging;

using Moq;

namespace Bookgen.Tests.Commands;

[TestFixture]
internal abstract class CommandTestBase<TCommand> where TCommand : ICommand
{
    protected readonly Mock<IFileSystem> FileSystemMock = new Mock<IFileSystem>(MockBehavior.Strict);
    protected readonly Mock<IValidationContext> ValidationContextMock = new Mock<IValidationContext>(MockBehavior.Strict);
    protected readonly Mock<ILogger> LoggerMock = new Mock<ILogger>(MockBehavior.Strict);
    protected readonly Mock<IAssetSource> AssetSourceMock = new Mock<IAssetSource>(MockBehavior.Strict);

    protected ICommand Command { get; private set; }

    [SetUp]
    public void Setup()
    {
        SetupMocks();
        Command = CreateSut();
    }

    protected abstract TCommand CreateSut();

    protected abstract void SetupMocks();
}
