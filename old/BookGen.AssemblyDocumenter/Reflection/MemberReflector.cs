//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Reflection;

namespace BookGen.AssemblyDocumenter.Reflection;

/// <summary>
/// Reflection helpers for a single <see cref="MemberInfo"/>.
/// </summary>
internal abstract class MemberReflector
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberReflector"/> class.
    /// </summary>
    /// <param name="member">The member, or null to use default properties.</param>
    public MemberReflector(MemberInfo? member)
    {
        try
        {
            IsBrowsable = member?.GetCustomAttribute<EditorBrowsableAttribute>()?.State != EditorBrowsableState.Never;
        }
        catch (IOException)
        {
            // Ignore members that are missing dependencies.
            IsBrowsable = true;
            System.Diagnostics.Trace.WriteLine($"Warning: unable to use reflection to determine browsable state for {member?.DeclaringType}.{member?.Name}");
        }
    }

    /// <summary>
    /// Gets a value indicating whether the member is intended to be discovered outside the assembly.
    /// </summary>
    public bool IsBrowsable { get; }

    /// <summary>
    /// Gets a value indicating whether the member is visible outside of the assembly.
    /// </summary>
    public abstract bool IsVisible { get; }
}