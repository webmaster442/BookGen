using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.Internals;

using BookGen.Vfs;

using Moq;

namespace Bookgen.Tests.Lib;

[TestFixture]
internal class UT_SerializedObjectValidator
{
    private Mock<IReadOnlyFileSystem> _fileSystem;
    private SerializedObjectValidator _validator;

    [SetUp]
    public void Setup()
    {
        _fileSystem = new Mock<IReadOnlyFileSystem>(MockBehavior.Strict);
        _fileSystem.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

        _validator = new SerializedObjectValidator(_fileSystem.Object);
    }

    [Test]
    public void Ensure_That_Validate_Partial_Config()
    {
        var config = new Config()
        {
            OutputFolder = "output",
            TocFile = "toc.json",
        };

        var issues = new List<string>();

        bool result = _validator.Validate(config, issues);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(issues, Is.Not.Empty);
        });
    }
}
