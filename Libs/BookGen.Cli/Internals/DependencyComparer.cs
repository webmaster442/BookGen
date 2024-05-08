//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;

namespace BookGen.Cli.Internals;

internal sealed class DependencyComparer : IComparer<Type>
{
    private readonly IResolver _resolver;

    public DependencyComparer(IResolver resolver)
    {
        _resolver = resolver;
    }

    public static ConstructorInfo? GetConstructor(Type type)
    {
        return type
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .OrderByDescending(x => x.GetParameters())
            .FirstOrDefault();
    }

    private static IEnumerable<Type> GetConstructorArgumentTypes(Type? type)
    {
        if (type == null)
            return Enumerable.Empty<Type>();

        ConstructorInfo? ctor = GetConstructor(type);

        return ctor == null
            ? throw new InvalidOperationException($"{type.FullName} doesn't have a public constructor")
            : ctor.GetParameters().Select(x => x.ParameterType);
    }

    public int Compare(Type? x, Type? y)
    {
        int xArgsResolvable = GetConstructorArgumentTypes(x).Count(_resolver.CanResolve);
        int yArgsResolvable = GetConstructorArgumentTypes(y).Count(_resolver.CanResolve);

        return xArgsResolvable.CompareTo(yArgsResolvable);
    }
}
