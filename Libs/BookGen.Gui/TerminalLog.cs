//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using Spectre.Console;
using System.Diagnostics;
using Webmaster442.HttpServerFramework;

namespace BookGen.Gui;

public sealed class TerminalLog : IServerLog, ILog
{
    private readonly TextWriter? _logFile;

    public TerminalLog(bool logFile, LogLevel level = LogLevel.Info)
    {
        LogLevel = level;
        if (logFile)
            _logFile = File.CreateText("BookGen.log");
    }

    private static string GetFormat(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Warning => "[yellow]",
            LogLevel.Info => "[white]",
            LogLevel.Critical => "[red]",
            _ => "[white]",
        };
    }

    public LogLevel LogLevel { get; set; }

    public event EventHandler<LogEventArgs>? OnLogWritten;

    public void Log(LogLevel logLevel, string format, params object[] args)
    {
        string text = string.Format(format, args).EscapeMarkup();
        string line = string.Format("{0} | {1} | {2}", DateTime.Now.ToShortTimeString(), logLevel, text).EscapeMarkup();
        if (logLevel <= LogLevel)
        {
            AnsiConsole.MarkupLine($"{GetFormat(logLevel)}{line}[/]");
            _logFile?.WriteLine(line);
        }
#if DEBUG
        else
        {
            Debug.WriteLine(line);
            _logFile?.WriteLine(line);
        }
#endif
        OnLogWritten?.Invoke(this, new LogEventArgs(logLevel, line));
    }

    public void Flush()
    {
        if (_logFile != null)
        {
            _logFile.Flush();
            _logFile.Close();
        }
    }

    public void PrintLine(string str)
    {
        AnsiConsole.WriteLine(str);
    }

    public void PrintLine(object obj)
    {
        AnsiConsole.WriteLine(obj.ToString() ?? "");
    }

    void ILog.Critical(Exception ex)
    {
        AnsiConsole.WriteLine("");
        AnsiConsole.WriteException(ex);
    }

    void IServerLog.Critical(Exception ex)
    {
        AnsiConsole.WriteLine("");
        AnsiConsole.WriteException(ex);
    }

    void IServerLog.Info(string format, params object[] args)
        => Log(LogLevel.Info, format, args);

    void IServerLog.Warning(string format, params object[] args)
        => Log(LogLevel.Warning, format, args);

}
