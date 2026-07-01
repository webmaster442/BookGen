//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib;
using Bookgen.Lib.AppSettings;
using Bookgen.Lib.Pipeline;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure.Loging;
using BookGen.Vfs;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BookGen;

[ExitCode(ExitCodes.Success, "The Book was built successfully.")]
[ExitCode(ExitCodes.ConfigError, "The configuration was invalid.")]
[ExitCode(ExitCodes.GeneralError, "An error occurred during the build.")]
internal abstract class BuildCommandBase : AsyncCommand<BuildArguments>
{
    protected readonly IWritableFileSystem _soruce;
    protected readonly IWritableFileSystem _target;
    protected readonly ILogger _logger;
    protected readonly IAssetSource _assetSource;
    protected readonly IMemoryCache _memoryCache;
    private readonly IProgramPathResolver _programPathResolver;

    public BuildCommandBase(IWritableFileSystem soruce,
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

    public abstract Pipeline GetPipeLine();

    public override async Task<int> ExecuteAsync(BuildArguments arguments, IReadOnlyList<string> context)
    {
        if (_target.DirectoryExists(arguments.OutputDirectory))
        {
            _target.Delete(arguments.OutputDirectory);
        }

        _target.CreateDirectoryIfNotExist(arguments.OutputDirectory);

        _soruce.Scope = arguments.Directory;
        _target.Scope = arguments.OutputDirectory;

        using var env = new BookEnvironment(_soruce, _target, _programPathResolver, _assetSource);
        EnvironmentStatus status = await env.Initialize(arguments.ConfigOverlay);

        if (!status.IsOk)
        {
            _logger.EnvironmentStatus(status);
            return ExitCodes.ConfigError;
        }

        if (!string.IsNullOrEmpty(arguments.HostOverride))
        {
            env.Configuration.StaticWebsiteConfig.DeployHost = arguments.HostOverride;
            env.Configuration.WordpressConfig.DeployHost = arguments.HostOverride;
        }

        Pipeline pipeline = GetPipeLine();


        bool result = await pipeline.ExecuteAsync(env, _logger, CancellationToken.None);

        return result ? ExitCodes.Success : ExitCodes.GeneralError;
    }
}
