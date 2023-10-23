//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Reflection;

namespace BookGen.AssemblyDocumenter.Reflection;
/// <summary>
/// Reflection helpers for an <see cref="Assembly"/>.
/// </summary>
internal class AssemblyReflector
{
    private readonly Assembly? _assembly;
    private readonly Dictionary<string, TypeReflector?> _typeCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyReflector"/> class.
    /// </summary>
    /// <param name="assembly">The assembly, or null to disable reflection.</param>
    internal AssemblyReflector(Assembly? assembly)
    {
        _typeCache = new Dictionary<string, TypeReflector?>();
        if (assembly != null)
        {
            _assembly = assembly;
        }
    }

    /// <summary>
    /// Gets a type from the assembly.
    /// </summary>
    /// <param name="name">The name of the type.</param>
    /// <returns>The type.</returns>
    public TypeReflector? GetType(string name)
    {
        return GetType(name, null);
    }

    /// <summary>
    /// Gets a type from the assembly.
    /// </summary>
    /// <param name="name">The name of the type.</param>
    /// <param name="parent">The parent type if <paramref name="name"/> refers to a nested type.</param>
    /// <returns>The type.</returns>
    private TypeReflector? GetType(string name, TypeReflector? parent)
    {
        if (_assembly == null || string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        if (!_typeCache.TryGetValue(name, out TypeReflector? type))
        {
            var realType = _assembly.GetType(name, throwOnError: false);

            if (realType == null)
            {
                // Determine if this is a nested type (which really have a + instead of . in the name).
                var lastDotIndex = name.LastIndexOf('.');
                var parentTypeName = name.Substring(0, lastDotIndex);
                var parentType = GetType(parentTypeName);

                if (parentType != null)
                {
                    var nestedName = parentType.FullName + "+" + name.Substring(lastDotIndex + 1);
                    var nestedType = GetType(nestedName, parentType);

                    if (nestedType != null)
                    {
                        _typeCache[name] = nestedType;
                        return nestedType;
                    }
                }

                Trace.WriteLine($"Warning: unable to use reflection to load type {name}");
                _typeCache[name] = null;
                return null;
            }

            type = new TypeReflector(realType, parent);
            _typeCache[name] = type;
        }

        return type;
    }
}
