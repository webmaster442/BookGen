//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Vfs;

public interface IFileSystemObserver : IDisposable
{
    event EventHandler<FileSystemChangeEventArgs>? FileChanged;
}
