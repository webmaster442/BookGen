using System.Diagnostics;
using System.Reflection;

namespace BookGen.AssemblyDocumenter.Reflection;

/// <summary>
/// Reflection helpers for a single <see cref="MethodBase"/>.
/// </summary>
internal class MethodReflector : MemberReflector
{
    private readonly MethodBase? method;

    /// <summary>
    /// Initializes a new instance of the <see cref="MethodReflector"/> class.
    /// </summary>
    /// <param name="method">The method, or null to use default properties.</param>
    public MethodReflector(MethodBase? method)
        : base(method)
    {
        this.method = method;

        try
        {
            this.Parameters = method?.GetParameters();

            // Visible if the method is public or protected in a non-sealed type.
            // Default to true if we don't have a method to check against.
            this.IsVisible = method != null
                ? method.IsPublic ||
                  (method.IsFamily && method.DeclaringType?.IsSealed == false) ||
                  (method.IsFamilyOrAssembly && method.DeclaringType?.IsSealed == false)
                : true;
        }
        catch (IOException)
        {
            // Ignore methods that are missing dependencies.
            Trace.WriteLine($"Warning: unable to use reflection to process method {method?.DeclaringType}.{method?.Name}");
        }
    }

    /// <inheritdoc/>
    public override bool IsVisible { get; }

    /// <summary>
    /// Gets the method's parameters, or null if the method could not be loaded.
    /// </summary>
    public ParameterInfo[]? Parameters { get; }
}