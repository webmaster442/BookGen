using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Domain.IO.Configuration;

using BookGen.Vfs;

namespace Bookgen.Lib.Pipeline;

public interface IBookEnvironment : IAssetSource, IDisposable
{
    Config Configuration { get; }
    TableOfContents TableOfContents { get; }
    IWritableFileSystem Source { get; }
    IWritableFileSystem Output { get; }

    public abstract static bool IsBookGenFolder(string folder);
}
