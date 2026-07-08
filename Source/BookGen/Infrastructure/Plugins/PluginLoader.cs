//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using BookGen.Api.V1;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Infrastructure.Plugins;

internal sealed class PluginLoader : IDisposable
{
    private readonly ILogger _logger;
    private readonly IReadOnlyFileSystem _readOnlyFileSystem;
    private readonly Dictionary<string, PluginLoadContext> _loadedContexts;
    private bool _disposed;


    public PluginLoader(ILogger logger, IReadOnlyFileSystem readOnlyFileSystem)
    {
        _logger = logger;
        _readOnlyFileSystem = readOnlyFileSystem;
        _loadedContexts = new Dictionary<string, PluginLoadContext>();
    }

    public void Dispose()
    {
        foreach (PluginLoadContext context in _loadedContexts.Values)
        {
            context.Unload();
        }
        _loadedContexts.Clear();
        _disposed = true;
    }

    public bool TryLoadPlugin(string filePath, [NotNullWhen(true)] out IBuildPluginV1? buildPlugin)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (!_readOnlyFileSystem.FileExists(filePath))
        {
            _logger.LogError("Plugin file does not exist: {FilePath}", filePath);
            buildPlugin = null;
            return false;
        }

        PluginLoadContext pluginLoadContext = new(filePath);
        Assembly assembly = pluginLoadContext.LoadFromAssemblyName(new(Path.GetFileNameWithoutExtension(filePath)));

        Type[] buildPluginTypes = assembly.GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IBuildPluginV1)))
            .ToArray();

        if (buildPluginTypes.Length == 0)
        {
            pluginLoadContext.Unload();
            _logger.LogError("No IBuildPlugin implementation found in assembly: {AssemblyName}", assembly.FullName);
            buildPlugin = null;
            return false;
        }

        if (buildPluginTypes.Length > 1)
        {
            pluginLoadContext.Unload();
            _logger.LogError("Multiple IBuildPlugin implementations found in assembly: {AssemblyName}", assembly.FullName);
            buildPlugin = null;
            return false;
        }

        try
        {
            buildPlugin = Activator.CreateInstance(buildPluginTypes[0]) as IBuildPluginV1;
            _loadedContexts.Add(filePath, pluginLoadContext);
            return buildPlugin != null;
        }
        catch (Exception ex)
        {
            pluginLoadContext.Unload();
            _logger.LogError(ex, "Failed to create instance of IBuildPlugin from assembly: {AssemblyName}", assembly.FullName);
            buildPlugin = null;
            return false;
        }
    }
}
