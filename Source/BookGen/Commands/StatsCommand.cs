using Bookgen.Lib;
using Bookgen.Lib.Domain;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure.Loging;
using BookGen.Infrastructure.Terminal;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

using Spectre.Console;

namespace BookGen.Commands;

[CommandName("stats")]
internal sealed class StatsCommand : AsyncCommand<BookGenArgumentBase>
{
    private readonly IWritableFileSystem _soruce;
    private readonly ILogger _logger;

    public StatsCommand(IWritableFileSystem soruce, ILogger logger)
    {
        _soruce = soruce;
        _logger = logger;
    }

    public override async Task<int> ExecuteAsync(BookGenArgumentBase arguments, IReadOnlyList<string> context)
    {
        _soruce.Scope = arguments.Directory;
        using var env = new BookEnvironment(_soruce, _soruce);

        var status = await env.Initialize(autoUpgrade: false);

        if (!status.IsOk)
        {
            _logger.EnvironmentStatus(status);
            return ExitCodes.ConfigError;
        }

        BookStat stats = await BookStatFactory.CreateBookStat(env, _logger);

        var table = new Table();
        table.AddColumns("Property", "Value");
        table.AddRow("Line Count", stats.LineCount.ToString("N0"));
        table.AddRow("Word Count", stats.WordCount.ToString("N0"));
        table.AddRow("Character Count", stats.CharacterCount.ToString("N0"));
        table.AddRow("Total File Size", stats.TotalSize.ToFileSize());
        AnsiConsole.Write(table);

        Terminal.BreakDownChart(stats.ChapterSizes, "Chapter sizes");
        Terminal.BreakDownChart(stats.FileCountsByExtension, "File counts by extension");
        Terminal.BreakDownChart(stats.FileSizeByExtension, "File sizes by extension");

        return ExitCodes.Succes;
    }
}
