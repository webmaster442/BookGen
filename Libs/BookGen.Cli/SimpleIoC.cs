﻿//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli.Internals;

namespace BookGen.Cli;

public sealed class SimpleIoC : IResolver, IDisposable
{
    private readonly Dictionary<Type, Type> _instanceTypes;
    private readonly Dictionary<Type, Type> _singletonTypes;
    private readonly Dictionary<Type, object> _singletons;

    public SimpleIoC()
    {
        _instanceTypes = [];
        _singletonTypes = [];
        _singletons = [];
    }

    public void Register<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : class
    {
        _instanceTypes.Add(typeof(TInterface), typeof(TImplementation));
    }

    public void Register<TImplementation>()
        where TImplementation : class
    {
        _instanceTypes.Add(typeof(TImplementation), typeof(TImplementation));
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
        var ctorToCall = DependencyComparer.GetConstructor(value)
            ?? throw new InvalidOperationException($"{value.FullName} doesn't have a public constructor");

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
        if (_instanceTypes.TryGetValue(type, out Type? foundType))
            return CreateInstance(foundType);

        if (_singletons.TryGetValue(type, out object? instance))
            return instance;

        throw new InvalidOperationException($"Don't know how to resolve: {type.FullName}");
    }

    public void Dispose()
    {
        foreach (var singleton in _singletons)
        {
            if (singleton.Value is IDisposable disposable)
                disposable.Dispose();
        }
    }
}
