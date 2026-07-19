using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

using BookGen.Api.V1;

using Microsoft.Extensions.Logging;

namespace BookGen.Infrastructure.Plugins;

internal sealed class PluginRunner
{
    private sealed class PluginTempFolder : IDisposable
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

    private static bool TryExtractArchive(ILogger logger, ZipArchive archive, string fullPath)
    {
        try
        {
            archive.ExtractToDirectory(fullPath, overwriteFiles: true);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to extract plugin package to temporary folder: {TempFolder}", fullPath);
            return false;
        }
    }

    private static bool ContainsPluginType(Assembly assembly, ILogger logger, [NotNullWhen(true)] out Type? pluginType)
    {
        Type[] buildPluginTypes = assembly.GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IBuildPluginV1)))
            .ToArray();

        if (buildPluginTypes.Length == 0)
        {
            logger.LogDebug("No IBuildPlugin implementation found in assembly: {AssemblyName}", assembly.FullName);
            pluginType = null;
            return false;
        }

        if (buildPluginTypes.Length > 1)
        {
            logger.LogError("Multiple IBuildPlugin implementations found in assembly: {AssemblyName}", assembly.FullName);
            pluginType = null;
            return false;
        }

        pluginType = buildPluginTypes[0];
        return true;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static Task<bool> RunPlugin(ILogger logger,
                                       IBook book,
                                       IBookgenServices bookgenServices,
                                       string pluginPath,
                                       bool isDevMode,
                                       CancellationToken cancellationToken)
    {
        string entryAssemblyPath = pluginPath;

        using (var tempFolder = new PluginTempFolder())
        {
            if (!isDevMode)
            {
                using ZipArchive archive = ZipFile.OpenRead(entryAssemblyPath);
                if (!archive.TryGetPluginManifest(entryAssemblyPath, logger, out PackageManifest? manifest))
                {
                    return Task.FromResult(false);
                }

                if (!TryExtractArchive(logger, archive, tempFolder.FullPath))
                {
                    return Task.FromResult(false);
                }

                entryAssemblyPath = Path.Combine(tempFolder.FullPath, manifest.EntryAssembly);
            }

            if (!File.Exists(entryAssemblyPath))
            {
                logger.LogError("Entry assembly specified in manifest does not exist: {EntryAssemblyPath}", entryAssemblyPath);
                return Task.FromResult(false);
            }

            (bool result, WeakReference loadContextWeakRef) = LoadAndExecutePlugin(logger, book, bookgenServices, entryAssemblyPath, cancellationToken);

            for (int i = 0; loadContextWeakRef.IsAlive && (i < 100); i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            if (loadContextWeakRef.IsAlive)
            {
                logger.LogWarning("Plugin load context is still alive after unloading: {EntryAssemblyPath}", entryAssemblyPath);
                Debugger.Break();
            }

            return Task.FromResult(result);
        }
    }

    // IMPORTANT: This method must NOT be async and must stay NoInlining.
    // A collectible AssemblyLoadContext can only be unloaded once there are no
    // references (managed or on the stack) to the context or to any type/object
    // from the loaded assembly. If this method were async, the compiler would
    // hoist the locals (loadContext, assembly, pluginType, pluginInstance,
    // buildPlugin, the plugin's Task) into a heap-allocated state machine whose
    // awaiter fields are not cleared after completion, keeping the ALC rooted.
    // Keeping it synchronous ensures all those references live on the stack and
    // are released the moment the method returns, so the GC loop in the caller
    // can collect the context. Only a bool and a WeakReference are handed back.
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (bool result, WeakReference loadContextWeakRef) LoadAndExecutePlugin(ILogger logger,
                                                                                        IBook book,
                                                                                        IBookgenServices bookgenServices,
                                                                                        string entryAssemblyPath,
                                                                                        CancellationToken cancellationToken)
    {
        var loadContext = new PluginLoadContext(entryAssemblyPath);
        var loadContextWeakRef = new WeakReference(loadContext);

        try
        {
            Assembly assembly = loadContext.LoadFromAssemblyPath(entryAssemblyPath);
            if (ContainsPluginType(assembly, logger, out Type? pluginType))
            {
                object? pluginInstance = Activator.CreateInstance(pluginType);
                if (pluginInstance is IBuildPluginV1 buildPlugin)
                {
                    bool result = buildPlugin.Build(book, bookgenServices, cancellationToken).GetAwaiter().GetResult();
                    return (result, loadContextWeakRef);
                }
            }

            return (false, loadContextWeakRef);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load plugin assembly: {EntryAssemblyPath}", entryAssemblyPath);
            return (false, loadContextWeakRef);
        }
        finally
        {
            loadContext.Unload();
        }
    }
}
