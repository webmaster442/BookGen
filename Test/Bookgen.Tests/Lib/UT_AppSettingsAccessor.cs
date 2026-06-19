using Bookgen.Lib.Confighandling;

using BookGen.Vfs;

using Moq;

namespace Bookgen.Tests.Lib;

[TestFixture]
public class UT_AppSettingsAccessor
{
    private Mock<IWritableFileSystem> _fileSystem;
    private AppSettingsAccessor _sut;

    [SetUp]
    public void Setup()
    {
        _fileSystem = new Mock<IWritableFileSystem>(MockBehavior.Strict);
        _fileSystem.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);
        _sut = new AppSettingsAccessor(_fileSystem.Object);
    }

    [Test]
    public void EnsureThat_GetWorks()
    {
        var value = _sut.Get(x => x.Editor);
        bool isValid = _sut.IsSettingValid(x => x.Editor, out IReadOnlyList<string> issues);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(value, Is.EqualTo("notepad.exe"));
            Assert.That(isValid, Is.True);
            Assert.That(issues, Is.Empty);
        }
    }
}
