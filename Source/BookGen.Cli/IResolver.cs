//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace BookGen.Cli;

public interface IResolver
{
    bool CanResolve(Type type);
    public object Resolve(Type type, [CallerMemberName]string? caller = null);
}
