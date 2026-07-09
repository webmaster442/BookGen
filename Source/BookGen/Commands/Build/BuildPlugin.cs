using System.ComponentModel;

using BookGen.Api.V1;
using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Cli.OpenCli.Draft;
using BookGen.Infrastructure.Loging;
using BookGen.Infrastructure.Plugins;
using BookGen.Infrastructure.Plugins.V1;
using BookGen.Lib;
using BookGen.Lib.AppSettings;
using BookGen.Vfs;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BookGen.Commands.Build;

internal sealed class BuildPlugin : AsyncCommand<BuildPlugin.Arguments>
{
    internal sealed class Arguments : ArgumentsBase, IVerbosablityToggle
    {
        [Argument(0, IsOptional = false)]
        [Description("Required argument. Specifies the plugin DLL file name.")]
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

            if (!PluginFile.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
                return ValidationResult.Error($"Plugin file '{PluginFile}' is not a valid DLL file.");
            }
        
            return ValidationResult.Ok();
        }
    }

    private readonly IWritableFileSystem _soruce;
    private readonly IWritableFileSystem _target;
    private readonly IProgramPathResolver _programPathResolver;
    private readonly ILogger _logger;
    private readonly IAssetSource _assetSource;
    private readonly IMemoryCache _memoryCache;



    public BuildPlugin(IWritableFileSystem soruce,
                       IWritableFileSystem target,
                       IProgramPathResolver programPathResolver,
                       ILogger logger,
                       IAssetSource assetSource,
                       IMemoryCache memoryCache)
    {
        _soruce = soruce;
        _target = target;
        _programPathResolver = programPathResolver;
        _logger = logger;
        _assetSource = assetSource;
        _memoryCache = memoryCache;
    }

    public override async Task<int> ExecuteAsync(Arguments arguments, IReadOnlyList<string> context, CancellationToken token)
    {
        using var loader = new PluginLoader(_logger, _soruce);
        if (!loader.TryLoadPlugin(arguments.PluginFile, out IBuildPluginV1? plugin))
        {
            _logger.LogError("Failed to load plugin from file '{PluginFile}'", arguments.PluginFile);
            return ExitCodes.PluginError;
        }

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
        IBookgenServices services = new BookGenServices(env, _memoryCache, _logger);

        await plugin.Build(book, services, CancellationToken.None);

        return ExitCodes.Success;

    }
}
