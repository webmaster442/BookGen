//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace BookGen.Framework;

/// <summary>
/// A developer exception indicating bad dependecy setup
/// </summary>
[Serializable]
public class DependencyException : Exception
{
    public DependencyException() : base()
    {
    }

    public DependencyException(string? message) : base(message)
    {
    }

    public DependencyException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument == null)
            throw new DependencyException($"{paramName} was not set");
    }
}
