using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.VFS;

namespace Bookgen.Lib.Pipeline;

public interface IEnvironment : IDisposable
{
    Config Configuration { get; }
    TableOfContents TableOfContents { get; }
    IFolder Source { get; }
}
