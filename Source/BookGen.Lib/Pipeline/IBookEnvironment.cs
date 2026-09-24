//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Lib.Domain.IO;
using BookGen.Lib.Domain.IO.Configuration;

using BookGen.Vfs;

namespace BookGen.Lib.Pipeline;

public interface IBookEnvironment : IAssetSource, IDisposable
{
    Config Configuration { get; }
    TableOfContents TableOfContents { get; }
    IWritableFileSystem Source { get; }
    IWritableFileSystem Output { get; }
    IProgramPathResolver ProgramPathResolver { get; }
    public abstract static bool IsBookGenFolder(string folder);
}
