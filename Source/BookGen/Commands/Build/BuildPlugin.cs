//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Api.V1;
using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Cli.OpenCli.Draft;
using BookGen.Infrastructure;
using BookGen.Infrastructure.Loging;
using BookGen.Infrastructure.Plugins;
using BookGen.Infrastructure.Plugins.V1;
using BookGen.Lib;
using BookGen.Lib.AppSettings;
using BookGen.Vfs;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BookGen.Commands.Build;

[CommandName("build plugin")]
[Description("Builds a book using a plugin nuget file.")]
[ExitCode(ExitCodes.Success, "The book was built successfully.")]
[ExitCode(ExitCodes.PluginError, "Failed to load the plugin.")]
[ExitCode(ExitCodes.ConfigError, "Failed to initialize the book environment.")]
internal sealed class BuildPlugin : AsyncCommand<BuildPlugin.Arguments>
{
    internal sealed class Arguments : ArgumentsBase, IVerbosablityToggle
    {
        [Argument(0, IsOptional = false)]
        [Description("Required argument. Specifies the plugin zip file name.")]
        public string PluginFile { get; set; } = string.Empty;

        [Switch("v", "verbose", Required = false)]
        [Description("Optional argument, turns on detailed logging. Usefull for locating issues")]
        public bool Verbose { get; set; }

        [Switch("d", "dir", Required = true)]
        [Description("Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.")]
        public string Directory { get; set; }

        [Switch("o", "output", Required = true)]
        [Description("Required argument. Specifies the output directory name.")]
        public string OutputDirectory { get; set; } = string.Empty;

        [Switch("dev", "devmode", Required = false)]
        [Description("Optional argument. Enables developer mode. This allows loading plugins with dll file")]
        public bool IsDevMode { get; set; }

        public Arguments()
        {
            Directory = Environment.CurrentDirectory;
        }

        public override ValidationResult Validate(IValidationContext context)
        {
            if (!context.FileSystem.DirectoryExists(Directory))
            {
                return ValidationResult.Error($"Directory '{Directory}' does not exist.");
            }

            if (string.IsNullOrWhiteSpace(OutputDirectory))
            {
                return ValidationResult.Error("Output directory must not be empty.");
            }

            if (!context.FileSystem.FileExists(PluginFile))
            {
                return ValidationResult.Error($"Plugin file '{PluginFile}' does not exist.");
            }

            if (IsDevMode)
            {
                if (!PluginFile.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                {
                    return ValidationResult.Error($"Plugin file '{PluginFile}' is not a valid dll file.");
                }
            }
            else if (!PluginFile.EndsWith(".plugin", StringComparison.OrdinalIgnoreCase))
            {
                return ValidationResult.Error($"Plugin file '{PluginFile}' is not a valid plugin package.");
            }

            return ValidationResult.Ok();
        }
    }

    private readonly IWritableFileSystem _soruce;
    private readonly IWritableFileSystem _target;
    private readonly IProgramPathResolver _programPathResolver;
    private readonly ILogger _logger;
    private readonly Vfs.IAssetSource _assetSource;
    private readonly IMemoryCache _memoryCache;
    private readonly IDynamicDocumentGenerator _dynamicDocumentGenerator;

    public BuildPlugin(IWritableFileSystem soruce,
                       IWritableFileSystem target,
                       IProgramPathResolver programPathResolver,
                       ILogger logger,
                       Vfs.IAssetSource assetSource,
                       IMemoryCache memoryCache,
                       IDynamicDocumentGenerator dynamicDocumentGenerator)
    {
        _soruce = soruce;
        _target = target;
        _programPathResolver = programPathResolver;
        _logger = logger;
        _assetSource = assetSource;
        _memoryCache = memoryCache;
        _dynamicDocumentGenerator = dynamicDocumentGenerator;
    }

    public override async Task<int> ExecuteAsync(Arguments arguments, IReadOnlyList<string> context, CancellationToken token)
    {
        string pluginAssemblyPath = Path.GetFullPath(arguments.PluginFile, AppContext.BaseDirectory);
        _logger.LogInformation("Loading plugin from: {PluginAssemblyPath}", pluginAssemblyPath);

        using var loader = new PluginPackageLoader(_logger, _soruce);
        if (!loader.TryLoad(pluginAssemblyPath, arguments.IsDevMode, out IBuildPluginV1? plugin))
        {
            return ExitCodes.PluginError;
        }
        try
        {
            if (_target.DirectoryExists(arguments.OutputDirectory))
            {
                _target.Delete(arguments.OutputDirectory);
            }

            _target.CreateDirectoryIfNotExist(arguments.OutputDirectory);

            _soruce.Scope = arguments.Directory;
            _target.Scope = arguments.OutputDirectory;

            using var env = new BookEnvironment(_soruce, _target, _programPathResolver, _assetSource);
            EnvironmentStatus status = await env.Initialize(null);

            if (!status.IsOk)
            {
                _logger.EnvironmentStatus(status);
                return ExitCodes.ConfigError;
            }


            IBook book = Infrastructure.Plugins.V1.Book.CreateFrom(env, _logger);
            IBookgenServices services = new BookGenServices(env, _memoryCache, _logger, _dynamicDocumentGenerator);

            await plugin.Build(book, services, token);
        }
        finally
        {
            if (plugin is IDisposable disposablePlugin)
            {
                disposablePlugin.Dispose();
            }
        }
        return ExitCodes.Success;

    }
}
