using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace BookGen.Cli.CrashReporting;

public sealed class CrashDumpGenerator
{
    private sealed class CrashHtmlBuilder
    {
        private readonly JsonSerializerOptions _options;
        private readonly Exception _exception;

        private string _title = string.Empty;
        private string _programArgs = string.Empty;
        private string _osInformation = string.Empty;
        private string _applicationVersion = string.Empty;
        private string _applicationArchitecture = string.Empty;
        private Version _runtime = Version.Parse("0.0.0.0");
        private string _logEntries = string.Empty;
        private string _css;

        public CrashHtmlBuilder(Exception exception)
        {
            _options = new()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            };
            _exception = exception;
            _css = """
            body {
                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                background-color: #FCFCFC;
                max-width: 1400px;
                margin: 0 auto;
            }
            pre, code {
                font-family: 'Cascadia Code', monospace;
            }
            details {
                margin-left: 15px;
            }
            .exception-type {
                font-weight: bold;
                color: #E02A24;
            }
            .version {
                color: #53735D;
            }
            .reason {
                font-weight: bold;
                color: #C08CF3;
            }
            """;
        }

        public CrashHtmlBuilder Title(string title)
        {
            _title = title;
            return this;
        }

        public CrashHtmlBuilder ProgramArgs(string[] args)
        {
            _programArgs = string.Join(' ', args.Select(x => x.Contains(' ') ? $"\"{x}\"" : x));
            return this;
        }

        public CrashHtmlBuilder WithOperatingSystem(string osInformation)
        {
            _osInformation = osInformation;
            return this;
        }

        public CrashHtmlBuilder WithApplicationVersion(string version)
        {
            _applicationVersion = version;
            return this;
        }

        public CrashHtmlBuilder WithApplicationArchitecture(string architecture)
        {
            _applicationArchitecture = architecture;
            return this;
        }

        public CrashHtmlBuilder WithRuntime(Version version)
        {
            _runtime = version;
            return this;
        }

        public CrashHtmlBuilder WithLogEntries(IEnumerable<LogEntry>? logEntries)
        {
            if (logEntries != null)
            {
                _logEntries = JsonSerializer.Serialize(logEntries.ToArray(), _options);
            }
            return this;
        }

        public CrashHtmlBuilder Css(string cssContent)
        {
            _css = cssContent;
            return this;
        }

        public override string ToString()
        {
            StringBuilder html = new();
            html.AppendLine($"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="utf-8">
                <title>{_title}</title>
                <style type="text/css">
                    {_css}
                </style>
            </head>
            <body>
            <h1>{_title}</h1>
            <hr>
            <p><b>Application Arguments:</b> <code>{HttpUtility.HtmlEncode(_programArgs)}</code></p>
            <p><b>Application Version:</b> <code class="version">{HttpUtility.HtmlEncode(_applicationVersion)}</code></p>
            <p><b>Application Architecture:</b> <code>{HttpUtility.HtmlEncode(_applicationArchitecture)}</code></p>
            <p><b>Operating System:</b> <code>{HttpUtility.HtmlEncode(_osInformation)}</code></p>
            <p><b>Runtime Version:</b> <code class="version">{HttpUtility.HtmlEncode(_runtime.ToString())}</code></p>
            <p><b>Crash Reason: </b> <code class="reason">{HttpUtility.HtmlEncode(_exception.Message)}</code></p>
            """);

            if (_logEntries != string.Empty)
            {
                html.AppendLine("<hr>");
                html.AppendLine("<details open>");
                html.AppendLine($"<summary><h2>Recent Log Entries</h2></summary>");
                html.AppendLine($"<pre>{HttpUtility.HtmlEncode(_logEntries)}</pre>");
                html.AppendLine("</details>");
            }

            html
                .AppendLine("<hr>")
                .AppendLine("<h2>Exception details</h2>");

            WalkException(_exception, html);
            html.AppendLine("<hr>");

            html.AppendLine("""
            </body>
            </html>
            """);

            return html.ToString();
        }

        private static void WalkException(Exception exception, StringBuilder html)
        {
            html
                .AppendLine("<details open>")
                .AppendLine($"<summary><span class=\"exception-type\">{HttpUtility.HtmlEncode(exception.GetType().Name)}</span> - <span class=\"reason\">{HttpUtility.HtmlEncode(exception.Message)}</span></summary>")
                .AppendLine("<p><b>Stack trace:</b></p>")
                .AppendLine($"<pre>{HttpUtility.HtmlEncode(exception.StackTrace)}</pre>");

            Exception? inner = exception?.InnerException;
            int iopen = 0;
            do
            {
                if (inner == null)
                    break;

                html
                    .Append("<hr/>")
                    .Append("<p><b>Inner Exception:</b></p>")
                    .AppendLine("<details open>")
                    .AppendLine($"<summary><span class=\"exception-type\">{HttpUtility.HtmlEncode(inner.GetType().Name)}</span> - <span class=\"reason\">{HttpUtility.HtmlEncode(inner.Message)}</span></summary>")
                    .AppendLine("<p><b>Stack trace:</b></p>")
                    .AppendLine($"<pre>{HttpUtility.HtmlEncode(inner.StackTrace)}</pre>");

                iopen++;
                inner = inner.InnerException;
            }
            while (inner != null);

            for (int i = 0; i < iopen; i++)
            {
                html.AppendLine("</details>");
            }

            html.AppendLine("</details>");

        }
    }

    private readonly string _applicationName;
    private readonly string _targetDirectory;

    public CrashDumpGenerator(string applicationName, string? targetDirectory = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationName);
        _applicationName = applicationName;
        if (targetDirectory == null)
        {
            _targetDirectory = Environment.CurrentDirectory;
        }
        else
        {
            if (!Path.Exists(_targetDirectory))
                throw new ArgumentException($"The target directory '{_targetDirectory}' does not exist.", nameof(targetDirectory));

            _targetDirectory = targetDirectory;
        }
    }

    public void GenerateCrashDump(Exception ex, IEnumerable<LogEntry>? logEntries = null)
    {
        var fileName = Path.Combine(_targetDirectory, $"{_applicationName}_crash_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.html");

        CrashHtmlBuilder builder = new(ex);
        var html = builder
            .Title($"Crash Report - {_applicationName}")
            .ProgramArgs(Environment.GetCommandLineArgs())
            .WithOperatingSystem($"{RuntimeInformation.OSDescription} {RuntimeInformation.OSArchitecture}")
            .WithApplicationVersion(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown")
            .WithApplicationArchitecture(RuntimeInformation.ProcessArchitecture.ToString())
            .WithLogEntries(logEntries)
            .WithRuntime(Environment.Version)
            .ToString();

#pragma warning disable RCS1075
        try
        {
            File.WriteAllText(fileName, html);
        }
        catch (Exception)
        {
            // If writing the crash dump fails, we can't do much about it.
        }
#pragma warning restore RCS1075
    }
}
