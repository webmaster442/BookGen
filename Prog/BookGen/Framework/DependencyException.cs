//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------
using System.Runtime.Serialization;

namespace BookGen.Framework;

/// <summary>
/// A developer exception indicating bad dependecy setup
/// </summary>
[Serializable]
public class DependencyException : Exception
{
    public DependencyException()
    {
    }

    public DependencyException(string? message) : base("Missing dependecy for: " + message)
    {
    }

    public DependencyException(string? message, Exception? innerException) : base("Missing dependecy for: " + message, innerException)
    {
    }

    protected DependencyException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
