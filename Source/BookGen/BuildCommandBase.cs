using Bookgen.Lib;
using Bookgen.Lib.Pipeline;

using BookGen.Cli;
using BookGen.Commands;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen;

internal abstract class BuildCommandBase : AsyncCommand<BookGenArgumentBase>
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

    public override async Task<int> ExecuteAsync(BookGenArgumentBase arguments, IReadOnlyList<string> context)
    {
        _soruce.Scope = arguments.Directory;

        using var env = new BookEnvironment(_soruce, _target, _assetSource);
        var status = await env.Initialize();

        if (!status.IsOk)
        {
            foreach (var issue in status)
            {
                _logger.LogError(issue);
            }

            return ExitCodes.ConfigError;
        }

        Pipeline pipeline = GetPipeLine();


        bool result = await pipeline.ExecuteAsync(env, _logger, CancellationToken.None);

        return result ? ExitCodes.Succes : ExitCodes.GeneralError;
    }
}
