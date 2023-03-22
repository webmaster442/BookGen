//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli.Internals;

namespace BookGen.Cli;

public sealed class SimpleIoC : IResolver
{
    private readonly Dictionary<Type, Type> _instanceTypes;
    private readonly Dictionary<Type, Type> _singletonTypes;
    private readonly Dictionary<Type, object> _singletons;

    public SimpleIoC()
    {
        _instanceTypes = new Dictionary<Type, Type>();
        _singletonTypes = new Dictionary<Type, Type>();
        _singletons = new Dictionary<Type, object>();
    }

    public void Register<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : class
    {
        _instanceTypes.Add(typeof(TInterface), typeof(TImplementation));
    }

    public void RegisterSingleton<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : class
    {
        _singletonTypes.Add(typeof(TInterface), typeof(TImplementation));
    }

    public void RegisterSingleton<TInterface>(TInterface instance)
        where TInterface : class
    {
        _singletons.Add(typeof(TInterface), instance);
    }

    public bool CanResolve(Type type)
    {
        return _instanceTypes.ContainsKey(type)
            || _singletons.ContainsKey(type);
    }

    private object CreateInstance(Type value)
    {
        var ctorToCall = DependencyComparer.GetConstructor(value);

        if (ctorToCall == null)
            throw new InvalidOperationException($"{value.FullName} doesn't have a public constructor");

        var parameters = ctorToCall.GetParameters();
        object[] parameterInstances = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            parameterInstances[i] = Resolve(parameters[i].ParameterType);
        }

        return Activator.CreateInstance(value, parameterInstances)
            ?? throw new InvalidOperationException("Create instance failed");
    }

    public void Build()
    {
        var singletonsToBuild = _singletonTypes.OrderBy(x => x.Value, new DependencyComparer(this));
        foreach (var singleton in singletonsToBuild)
        {
            _singletons.Add(singleton.Key, CreateInstance(singleton.Value));
        }
        _singletonTypes.Clear();
    }

    public object Resolve(Type type)
    {
        if (_instanceTypes.ContainsKey(type))
            return CreateInstance(_instanceTypes[type]);

        if (_singletons.ContainsKey(type))
            return _singletons[type];

        throw new InvalidOperationException($"Don't know how to resolve: {type.FullName}");
    }
}
