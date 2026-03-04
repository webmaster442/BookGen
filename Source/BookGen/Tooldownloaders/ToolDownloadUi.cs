//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Infrastructure.Tools;

using Spectre.Console;

using Webmaster442.WindowsTerminal.Wigets;

namespace BookGen.Tooldownloaders;

internal sealed class ToolDownloadUi : IDownloadUi
{
    private class ExtendedProgresBar : Progressbar
    {
        private readonly string _message;
        private readonly long _maximum;

        public bool IsVisible { get; private set; }

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

        public override void OnHide()
        {
            base.OnHide();
            IsVisible = false;
        }

        public override void OnShow()
        {
            base.OnShow();
            IsVisible = true;
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
    {
        if (_progressBar != null && !_progressBar.IsVisible) return;
        _progressBar?.Hide();
    }
}
