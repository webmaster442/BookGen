//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.Loader;

using BookGen.Api.V1;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Infrastructure.Plugins;

internal sealed class PluginPackageLoader : IDisposable
{
    private sealed class PluginLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;

        public PluginLoadContext(string pluginPath) : base(isCollectible: true)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            return assemblyPath != null
                ? LoadFromAssemblyPath(assemblyPath)
                : null;
        }

        protected override nint LoadUnmanagedDll(string unmanagedDllName)
        {
            string? libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            return libraryPath != null
                ? LoadUnmanagedDllFromPath(libraryPath)
                : IntPtr.Zero;
        }
    }

    private sealed class PluginTempFolder: IDisposable
    {
        private readonly string _tempFolder;

        public PluginTempFolder()
        {
            var name = $"bookgen_plugin_{Random.Shared.Next():X16}";
            _tempFolder = Path.Combine(Path.GetTempPath(), name);
        }

        public string FullPath => _tempFolder;

        public void Dispose()
        {
            if (Directory.Exists(_tempFolder))
            {
                Directory.Delete(_tempFolder, recursive: true);
            }
        }
    }

    private readonly ILogger _logger;
    private readonly IReadOnlyFileSystem _readOnlyFileSystem;

    private PluginLoadContext? _pluginLoadContext;
    private PluginTempFolder? _pluginTempFolder;
    private bool _disposed;

    public PluginPackageLoader(ILogger logger, IReadOnlyFileSystem readOnlyFileSystem)
    {
        _logger = logger;
        _readOnlyFileSystem = readOnlyFileSystem;
    }

    public void Dispose()
    {
        _pluginLoadContext?.Unload();
        Thread.Sleep(100); // Give the unload a moment to complete
        _pluginTempFolder?.Dispose();
        _disposed = true;
    }

    private bool TryExtractArchive(ZipArchive archive, string fullPath)
    {
        try
        {
            archive.ExtractToDirectory(fullPath, overwriteFiles: true);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to extract plugin package to temporary folder: {TempFolder}", fullPath);
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

    private bool TryLoadDll(string entryAssemblyPath, out IBuildPluginV1? buildPlugin)
    {
        _pluginLoadContext = new PluginLoadContext(entryAssemblyPath);
        try
        {
            Assembly assembly = _pluginLoadContext.LoadFromAssemblyPath(entryAssemblyPath);
            if (ContainsPluginType(assembly, out Type? pluginType))
            {
                buildPlugin = Activator.CreateInstance(pluginType) as IBuildPluginV1;
                return buildPlugin != null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load plugin assembly: {EntryAssemblyPath}", entryAssemblyPath);
            buildPlugin = null;
            return false;
        }

        buildPlugin = null;
        return false;
    }


    public bool TryLoad(string packagePath, bool isDevMode, [NotNullWhen(true)] out IBuildPluginV1? buildPlugin)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        string? resolvedPath = PluginPathResolver.Resolve(Environment.CurrentDirectory, packagePath, isDevMode);
        if (string.IsNullOrEmpty(resolvedPath))
        {
            buildPlugin = null;
            return false;
        }

        string entryAssemblyPath = resolvedPath;

        if (!isDevMode)
        {
            using ZipArchive archive = ZipFile.OpenRead(resolvedPath);
            if (!archive.TryGetPluginManifest(resolvedPath, _logger, out PackageManifest? manifest))
            {
                buildPlugin = null;
                return false;
            }

            _pluginTempFolder = new PluginTempFolder();
            if (!TryExtractArchive(archive, _pluginTempFolder.FullPath))
            {
                buildPlugin = null;
                return false;
            }

            entryAssemblyPath = Path.Combine(_pluginTempFolder.FullPath, manifest.EntryAssembly);
        }

        if (!File.Exists(entryAssemblyPath))
        {
            _logger.LogError("Entry assembly specified in manifest does not exist: {EntryAssemblyPath}", entryAssemblyPath);
            buildPlugin = null;
            return false;
        }

        return TryLoadDll(entryAssemblyPath, out buildPlugin);
    }
}
