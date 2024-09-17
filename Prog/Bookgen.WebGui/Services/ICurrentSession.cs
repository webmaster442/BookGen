//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;

namespace BookGen.WebGui.Services;

public interface ICurrentSession
{
    FsPath StartDirectory { get; }
    FsPath AppDirectory { get; }
}