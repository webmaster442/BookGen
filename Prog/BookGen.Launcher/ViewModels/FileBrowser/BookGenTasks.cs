using System.Collections;

namespace BookGen.Launcher.ViewModels.FileBrowser;

internal sealed class BookGenTasks : IEnumerable<BookGenTask>
{
    public IEnumerator<BookGenTask> GetEnumerator()
    {
        yield return new BookGenTask
        {
            Name = "Update tags...",
            Command = "tags"
        };
        yield return new BookGenTask
        {
            Name = "Build test website...",
            Command = "build -a Test",
        };
        yield return new BookGenTask
        {
            Name = "Build website...",
            Command = "build -a BuildWeb",
        };
        yield return new BookGenTask
        {
            Name = "Build e-pub...",
            Command = "build -a BuildEpub",
        };
        yield return new BookGenTask
        {
            Name = "Build printable...",
            Command = "build -a BuildPrint",
        };
        yield return new BookGenTask
        {
            Name = "Build wordpress xml...",
            Command = "build -a BuildWordpress",
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}