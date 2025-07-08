using Spectre.Console;

using Webmaster442.WindowsTerminal.Wigets;

namespace BookGen.Tooldownloaders;

internal class ToolDownloadUi: IDownloadUi
{
    private class ExtendedProgresBar : Progressbar
    {
        private readonly string _message;
        private readonly long _maximum;

        public ExtendedProgresBar(string message, long maximum)
        {
            _message = message;
            _maximum = maximum;
        }

        protected override void OnProgressChanged()
        {
            base.OnProgressChanged();
            AnsiConsole.WriteLine(_message);
        }

        public void Report(long value)
        {
            double progress = (double)value / _maximum;
            base.Report(progress);
        }
    }

    private ExtendedProgresBar? _progressBar;

    public void BeginNew(string message, long maximum)
    {
        _progressBar = new ExtendedProgresBar(message, maximum);
        _progressBar.Show(true);
    }

    public void Error(string message)
    {
        _progressBar?.Hide();
        AnsiConsole.MarkupLine($"[red]Error:[/] {message}");
    }

    public void Report(long value)
        => _progressBar?.Report(value);

    internal void End()
        => _progressBar?.Hide();
}
