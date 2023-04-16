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
        this.IsVisible = field != null ? (field.IsPublic || field.IsFamily || field.IsFamilyOrAssembly) : true;
    }

    /// <inheritdoc/>
    public override bool IsVisible { get; }
}
