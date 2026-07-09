//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Reflection;

using BookGen.Api.V1;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Infrastructure.Plugins;

internal sealed class PluginLoader : IDisposable
{
    private readonly ILogger _logger;
    private readonly IReadOnlyFileSystem _readOnlyFileSystem;
    private PluginLoadContext? _pluginLoadContext;
    private NuGetTempFolder? _nuGetTempFolder;
    private bool _disposed;

    private const string CompatibleFrameworkMoniker = "netstandard2.1";

    public PluginLoader(ILogger logger, IReadOnlyFileSystem readOnlyFileSystem)
    {
        _logger = logger;
        _readOnlyFileSystem = readOnlyFileSystem;
    }

    public void Dispose()
    {
        _pluginLoadContext?.Unload();
        _nuGetTempFolder?.Dispose();
        _disposed = true;
    }

    private bool TryExtractNuget(string filePath, string destination)
    {
        using ZipArchive archive = ZipFile.OpenRead(filePath);
        bool canLoad = false;
        foreach (var entry in archive.Entries)
        {
            if (entry.FullName.StartsWith($"lib\\{CompatibleFrameworkMoniker}", StringComparison.OrdinalIgnoreCase)
                || entry.FullName.StartsWith($"lib/{CompatibleFrameworkMoniker}", StringComparison.OrdinalIgnoreCase))
            {
                canLoad = true;
                break;
            }
        }

        if (!canLoad)
        {
            _logger.LogError("NuGet package does not contain a compatible assembly for {CompatibleFrameworkMoniker}: {FilePath}", CompatibleFrameworkMoniker, filePath);
            return false;
        }

        try
        {
            ZipFile.ExtractToDirectory(filePath, destination, overwriteFiles: true);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to extract NuGet package: {FilePath}", filePath);
            return false;
        }
    }

    private bool ContainsPluginType(Assembly assembly, [NotNullWhen(true)] out Type? pluginType)
    {
        Type[] buildPluginTypes = assembly.GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IBuildPluginV1)))
            .ToArray();

        if (buildPluginTypes.Length == 0)
        {
            _logger.LogDebug("No IBuildPlugin implementation found in assembly: {AssemblyName}", assembly.FullName);
            pluginType = null;
            return false;
        }

        if (buildPluginTypes.Length > 1)
        {
            _logger.LogError("Multiple IBuildPlugin implementations found in assembly: {AssemblyName}", assembly.FullName);
            pluginType = null;
            return false;
        }

        pluginType = buildPluginTypes[0];
        return true;
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

        _nuGetTempFolder = new NuGetTempFolder(filePath);

        if (!TryExtractNuget(filePath, _nuGetTempFolder))
        {
            buildPlugin = null;
            return false;
        }

        _pluginLoadContext = new(_nuGetTempFolder.GetFrameworkPath(CompatibleFrameworkMoniker));
        foreach (string dllFile in _nuGetTempFolder.GetDllFiles(CompatibleFrameworkMoniker))
        {
            try
            {
                Assembly assembly = _pluginLoadContext.LoadFromAssemblyPath(dllFile);
                if (ContainsPluginType(assembly, out Type? pluginType))
                {
                    buildPlugin = Activator.CreateInstance(pluginType) as IBuildPluginV1;
                    return buildPlugin != null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Error while loading assembly: {DllFile}, Error: {Message}", dllFile, ex.Message);
            }
        }

        buildPlugin = null;
        return false;
    }
}
