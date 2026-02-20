//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure.Terminal;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("templates")]
internal class TemplatesCommand : AsyncCommand<TemplatesCommand.TemplatesArguments>
{
    internal class TemplatesArguments : ArgumentsBase
    {
        [Switch("n", "name")]
        public string TemplateName { get; set; } = string.Empty;
    }

    private readonly ILogger _logger;
    private readonly IAssetSource _assetSource;
    private readonly IWritableFileSystem _writableFileSystem;
    private readonly string[] _defaultTemplates;

    public TemplatesCommand(ILogger logger, IAssetSource assetSource, IWritableFileSystem writableFileSystem)
    {
        _logger = logger;
        _assetSource = assetSource;
        _writableFileSystem = writableFileSystem;
        _defaultTemplates = ["Epub.html", "Md2Html.html", "Print.html", "Static.html"];
    }

    public override async Task<int> ExecuteAsync(TemplatesArguments arguments, IReadOnlyList<string> context)
    {
        if (string.IsNullOrEmpty(arguments.TemplateName))
        {
            ListTempates();
            return ExitCodes.Success;
        }

        if (_assetSource.TryGetAsset(arguments.TemplateName, out string? template))
        {
            await _writableFileSystem.WriteAllTextAsync(arguments.TemplateName, template);
            _logger.LogInformation("Template {template} was written to current folder", arguments.TemplateName);
            return ExitCodes.Success;
        }

        _logger.LogError("Template {template} was not found in default templates", arguments.TemplateName);
        return ExitCodes.GeneralError;
    }

    private void ListTempates()
    {
        Terminal.Header("Default templates:", blankLineBefore: 1);
        Terminal.List(_defaultTemplates);

        Terminal.Header("Single page templates:", blankLineBefore: 1);
        IOrderedEnumerable<string> singlePage = _assetSource.AssetNames.Where(n => n.EndsWith(".template", StringComparison.OrdinalIgnoreCase)).Order();
        Terminal.List(singlePage);
    }
}
