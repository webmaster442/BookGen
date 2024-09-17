//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;

namespace BookGen.WebGui.Services;

internal sealed class CurrentSession : ICurrentSession
{
    public FsPath StartDirectory { get; }

    public FsPath AppDirectory { get; }

    public CurrentSession(string directory)
    {
        StartDirectory = new FsPath(directory);
        AppDirectory = new FsPath(AppContext.BaseDirectory);
    }
}
