//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib;

using BookGen.Cli;

using Microsoft.Extensions.Logging;

namespace BookGen.Infrastructure;

internal class HelpProvider : IHelpProvider
{
    private readonly ILogger _log;
    private readonly ICommandRunnerProxy _commandNameProvider;
    private readonly Dictionary<string, string[]> _helpData;
    private readonly Dictionary<string, Func<string>> _callbackTable;

    public HelpProvider(ILogger log, ICommandRunnerProxy nameProvider)
    {
        _log = log;
        _commandNameProvider = nameProvider;
        _helpData = new Dictionary<string, string[]>();
        _callbackTable = new Dictionary<string, Func<string>>();

        LoadHelpData();
    }

    public IEnumerable<string> HelpEntries => _helpData.Keys;

    private void LoadHelpData()
    {
        var lines =
            ResourceHandler.GetResourceFileLines<HelpProvider>("Resources/Commands.md");

        List<string> chapterData = new(50);
        string? currentChapter = null;

        foreach (var line in lines)
        {
            if (line.StartsWith("# "))
            {
                if (chapterData.Count > 0
                    && !string.IsNullOrEmpty(currentChapter))
                {
                    _helpData.Add(currentChapter, chapterData.ToArray());
                    chapterData.Clear();
                }
                currentChapter = line[2..].Trim().ToLower();
                chapterData.Add(line);
            }
            else
            {
                chapterData.Add(line);
            }
        }
        if (chapterData.Count > 0
            && currentChapter != null
            && !_helpData.ContainsKey(currentChapter))
        {
            _helpData.Add(currentChapter, chapterData.ToArray());
        }


    }

    public void VerifyHelpData()
    {
        foreach (var name in _commandNameProvider.CommandNames)
        {
            if (!_helpData.ContainsKey(name))
            {
                _log.LogWarning("No help was found for command: {command}", name);
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif
            }
        }

    }

    public void RegisterCallback(string commandName, Func<string> callback)
    {
        _callbackTable.Add(commandName, callback);
    }

    public IEnumerable<string> GetCommandHelp(string cmd)
    {
        foreach (var line in _helpData[cmd])
        {
            yield return line;
        }
        if (_callbackTable.ContainsKey(cmd))
        {
            var lines = _callbackTable[cmd].Invoke().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                yield return line;
            }
        }
    }
}