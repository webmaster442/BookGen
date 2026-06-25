//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace Bookgen.Lib.AppSettings;

public interface IProgramPathResolver
{
    bool TryResolvePythonPath([NotNullWhen(true)] out string? path);
    bool TryResolveNodeJsPath([NotNullWhen(true)] out string? path);
    bool TryResolveRatex([NotNullWhen(true)] out string? path);
    bool TryResolveMmdr([NotNullWhen(true)] out string? path);
    bool TryResolvePlantUml([NotNullWhen(true)] out string? path);
}
