using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Domain.IO.Configuration;

using BookGen.Vfs;

namespace Bookgen.Lib.Pipeline;

public interface IEnvironment : IAssetSource, IDisposable
{
    Config Configuration { get; }
    TableOfContents TableOfContents { get; }
    ICache Cache { get; }
    IWritableFileSystem Source { get; }
    IWritableFileSystem Output { get; }
}
