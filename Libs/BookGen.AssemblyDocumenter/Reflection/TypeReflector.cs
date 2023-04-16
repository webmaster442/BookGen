//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace BookGen.AssemblyDocumenter.Reflection;

/// <summary>
/// Reflection helpers for a single <see cref="Type"/>.
/// </summary>
internal class TypeReflector
{
    private readonly Type _type;
    private ILookup<string, MethodReflector>? _methodCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeReflector"/> class.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="parent">The parent type if <paramref name="type"/> is a nested type, or null if not.</param>
    internal TypeReflector(Type type, TypeReflector? parent)
    {
        _type = type;
        IsVisible = type.IsVisible;
        Constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Select(c => new MethodReflector(c)).ToArray();

        try
        {
            this.IsBrowsable = type.GetCustomAttribute<EditorBrowsableAttribute>()?.State != EditorBrowsableState.Never;
        }
        catch (IOException)
        {
            // Ignore types that are missing dependencies.
            this.IsBrowsable = true;
            Trace.WriteLine($"Warning: unable to use reflection to determine properties for {type.FullName}");
        }

        if (parent != null)
        {
            this.IsVisible &= parent.IsVisible;
            this.IsBrowsable &= parent.IsBrowsable;
        }
    }

    /// <summary>
    /// Gets the constructors defined for the type.
    /// </summary>
    public MethodReflector[] Constructors { get; }

    /// <summary>
    /// Gets a value indicating whether the type is intended to be discovered outside the assembly.
    /// </summary>
    public bool IsBrowsable { get; }

    /// <summary>
    /// Gets a value indicating whether the type is visible outside the assembly.
    /// </summary>
    public bool IsVisible { get; }

    /// <summary>
    /// Gets the type's full name.
    /// </summary>
    public string? FullName => this._type.FullName;

    /// <summary>
    /// Gets the field with the given name.
    /// </summary>
    /// <param name="name">The name of the field.</param>
    /// <returns>The field.</returns>
    public FieldReflector GetField(string name) => new FieldReflector(_type.GetField(name));

    /// <summary>
    /// Returns all method overloads with the given name.
    /// </summary>
    /// <param name="name">The name of the methods to return.</param>
    /// <returns>The methods.</returns>
    public IEnumerable<MethodReflector> GetMethods(string name)
    {
        if (this._methodCache == null)
        {
            this._methodCache = this._type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                                    BindingFlags.Static | BindingFlags.DeclaredOnly)
                .ToLookup(m => m.Name, m => new MethodReflector(m));
        }

        // Explicit method implementations use # in the XML file but really are dot-separated.
        return this._methodCache[name.Replace('#', '.')];
    }

    /// <summary>
    /// Returns a method for accessing a property with the given name.
    /// </summary>
    /// <param name="name">The name of the property to return.</param>
    /// <returns>The accessor.</returns>
    public MethodReflector GetProperty(string name)
    {
        var propertyInfo = this._type.GetProperty(name);
        return new MethodReflector(propertyInfo?.GetMethod ?? propertyInfo?.SetMethod);
    }
}