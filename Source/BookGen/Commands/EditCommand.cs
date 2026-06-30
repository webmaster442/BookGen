//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using Bookgen.Lib.AppSettings;

using BookGen.Cli;
using BookGen.Cli.Annotations;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("edit")]
internal sealed class EditCommand : Command
{
    internal static class EditorHelper
    {
        private static readonly HashSet<string> SupportedFileTypes =
        [
            ".txt", ".md", ".js",
            ".json", ".yaml", ".html",
            ".htm", ".css", ".cmd",
            ".ps", ".sh", ".css",
            ".php", ".py", ".xml",
        ];

        public static bool IsSupportedFile(string file)
        {
            string? ext = Path.GetExtension(file).ToLower();
            return SupportedFileTypes.Contains(ext);
        }
    }

    private readonly ILogger _log;
    private readonly IReadOnlyAppSettings _appSettings;

    public EditCommand(ILogger log, IReadOnlyAppSettings appSettings)
    {
        _log = log;
        _appSettings = appSettings;
    }

    public override int Execute(IReadOnlyList<string> context)
    {
        if (context.Count != 1)
        {
            _log.LogWarning("No file name given");
            return ExitCodes.ArgumentsError;
        }

        if (string.IsNullOrEmpty(_appSettings.Get(x => x.Editor)))
        {
            _log.LogWarning("No Editor configured");
            return ExitCodes.ArgumentsError;
        }

        string? file = Path.GetFullPath(context[0]);

        if (!EditorHelper.IsSupportedFile(file))
        {
            _log.LogWarning("Unsupported file type");
            return ExitCodes.ArgumentsError;
        }

        try
        {
            using var p = new Process();
            p.StartInfo.FileName = _appSettings.Get(x => x.Editor);
            p.StartInfo.Arguments = $"\"{file}\"";
            p.StartInfo.UseShellExecute = false;
            p.Start();
            return ExitCodes.Success;
        }
        catch (Exception ex)
        {
            _log.LogCritical(ex, "Critical Error: {ex}", ex.Message);
            return ExitCodes.GeneralError;
        }
    }
}
