using Bookgen.Lib;
using Bookgen.Lib.Pipeline;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Commands;
using BookGen.Infrastructure.Loging;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen;

public sealed class BuildArguments : BookGenArgumentBase
{
    [Switch("o", "output")]
    public string OutputDirectory { get; set; } = string.Empty;

    [Switch("h", "host")]
    public string HostOverride { get; set; } = string.Empty;

    public override ValidationResult Validate(IValidationContext context)
    {
        var originalResult = base.Validate(context);
        if (originalResult.IsOk 
            && !string.IsNullOrEmpty(HostOverride)
            && !HostOverride.EndsWith('/'))
        {
            return ValidationResult.Error("Host override must end with a slash.");
        }
        return originalResult;
    }
}

internal abstract class BuildCommandBase : AsyncCommand<BuildArguments>
{
    protected readonly IWritableFileSystem _soruce;
    protected readonly IWritableFileSystem _target;
    protected readonly ILogger _logger;
    private readonly IAssetSource _assetSource;

    public BuildCommandBase(IWritableFileSystem soruce,
                            IWritableFileSystem target,
                            ILogger logger,
                            IAssetSource assetSource)
    {
        _soruce = soruce;
        _target = target;
        _logger = logger;
        _assetSource = assetSource;
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

        using var env = new BookEnvironment(_soruce, _target, _assetSource);
        EnvironmentStatus status = await env.Initialize(autoUpgrade: true);

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

        return result ? ExitCodes.Succes : ExitCodes.GeneralError;
    }
}
