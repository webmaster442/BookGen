//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Dotenv;

using Microsoft.Extensions.Logging;

namespace BookGen.GlobalOptionParsers;

internal class DotEnvFileParser : GlobalOptionParser
{
    private readonly ILogger _log;
    private readonly DotEnvSettings _dotEnvSettings;

    public DotEnvFileParser(ILogger log, DotEnvSettings dotEnvSettings)
        : base("env", "env-file", true)
    {
        _log = log;
        _dotEnvSettings = dotEnvSettings;
    }
    protected override void OnOptionWasPresent(string value)
    {
        _dotEnvSettings.Clear();
        var fileName = Path.GetFullPath(value);
        if (!File.Exists(fileName))
        {
            _log.LogWarning("The specified env-file '{fileName}' does not exist.", fileName);
            Environment.Exit(ExitCodes.GeneralError);
            return;
        }
        try
        {
            _log.LogInformation("Loading env-file '{fileName}'.", fileName);
            using StreamReader reader = File.OpenText(fileName);
            DotEnvSettings loaded = DotEnvParser.Parse(reader, StringComparer.Ordinal);
            _dotEnvSettings.Merge(loaded);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Failed to load env-file '{fileName}'.", fileName);
            Environment.Exit(ExitCodes.GeneralError);
        }
    }
}
