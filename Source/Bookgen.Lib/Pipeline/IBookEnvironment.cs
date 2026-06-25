//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.AppSettings;
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
    IProgramPathResolver ProgramPathResolver { get; }
    public abstract static bool IsBookGenFolder(string folder);
}
