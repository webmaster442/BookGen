//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;

namespace BookGen.AssemblyDocumenter.Reflection;

/// <summary>
/// Reflection helpers for a single <see cref="FieldInfo"/>.
/// </summary>
internal class FieldReflector : MemberReflector
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FieldReflector"/> class.
    /// </summary>
    /// <param name="field">The field, or null to use default properties.</param>
    public FieldReflector(FieldInfo? field)
        : base(field)
    {
        IsVisible = field == null || field.IsPublic || field.IsFamily || field.IsFamilyOrAssembly;
    }

    /// <inheritdoc/>
    public override bool IsVisible { get; }
}
