//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Lib;
using BookGen.Lib.Confighandling;
using BookGen.Lib.Domain.IO;
using BookGen.Lib.Domain.IO.Configuration;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands.Book;

[CommandName("book upgrade")]
[Description("Upgrades the bookgen project to the latest version. This command will upgrade the bookgen.json config file to the latest version, and will also upgrade the bookgen.toc.json file to the latest version.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
internal class UpgradeCommand : AsyncCommand<BookGenArgumentBase>
{
    private readonly ILogger _logger;
    private readonly IWritableFileSystem _source;

    public UpgradeCommand(ILogger logger, IWritableFileSystem source)
    {
        _logger = logger;
        _source = source;
    }

    public override async Task<int> ExecuteAsync(BookGenArgumentBase arguments, IReadOnlyList<string> context, CancellationToken token)
    {
        var upgrader = new ConfigUpgrader(_logger);
        _source.Scope = arguments.Directory;

        bool upgradeNeeded = await upgrader.Init(_source);
        if (!upgradeNeeded)
        {
            _logger.LogInformation("No upgrade needed. Configuration is up to date.");
            return ExitCodes.Success;
        }

        (bool tocModified, bool configmodified) = await upgrader.Upgrade(_source);

        if (tocModified)
        {
            await _source.WriteSchema<TableOfContents>(FileNameConstants.TableOfContentsSchema);
            _logger.LogInformation("Table of contents was upgraded.");
        }

        if (configmodified)
        {
            await _source.WriteSchema<Config>(FileNameConstants.ConfigFileSchema);
            _logger.LogInformation("Configuration file was upgraded.");
        }

        _logger.LogInformation("Configuration was upgraded from version {oldVersion} to {newVersion}. Please check the configuration files before running bookgen.", upgrader.VersionTag, Config.CurrentVersionTag);

        return ExitCodes.Success;
    }
}
