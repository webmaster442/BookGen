//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;

namespace BookGen.WebGui.Services;

internal sealed class CurrentSession : ICurrentSession
{
    public FsPath StartDirectory { get; set; }

    public FsPath AppDirectory { get; }

    public Version Version { get; }

    public CurrentSession()
    {
        StartDirectory = FsPath.Empty;
        AppDirectory = new FsPath(AppContext.BaseDirectory);
        Version = typeof(CurrentSession).Assembly.GetName().Version ?? new Version();
    }
}
