//-----------------------------------------------------------------------------
// (c) 2021-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli.Mediator;

using Spectre.Console;

using Webmaster442.WindowsTerminal;

using static BookGen.Gui.MessageTypes;

namespace BookGen.Gui;

public sealed class ConsoleProgressbar : IProgress<int>
{
    public int Maximum { get; }
    public bool Enabled { get; }

    private bool _alternateBuffer;

    private readonly TerminalFormattedStringBuilder _builder;
    private readonly IMediator _mediator;

    public ConsoleProgressbar(int maximum, bool enabled, IMediator mediator)
    {
        _builder = new();
        Maximum = maximum;
        Enabled = enabled;
        _mediator = mediator;
    }

    public void Report(int value)
    {
        if (!Enabled) return;
        DoReport(value);
    }

    public void SwitchBuffers()
    {
        if (!Enabled) return;
        _alternateBuffer = !_alternateBuffer;
        if (_alternateBuffer)
        {

            _mediator.Notify(new BeginLogRedirectMessage());
            WindowsTerminal.SwitchToAlternateBuffer();
            WindowsTerminal.SetProgressbar(ProgressbarState.Default, 0);
        }
        else
        {
            WindowsTerminal.SetProgressbar(ProgressbarState.Hidden, 0);
            WindowsTerminal.SwitchToMainBuffer();
            _mediator.Notify(new EndLogRedirectMessage());
        }
    }

    private void DoReport(int value)
    {
        AnsiConsole.Clear();

        int position = (Console.WindowHeight - 3) / 2;
        Console.SetCursorPosition(0, position);
        int percent = (int)Math.Ceiling(((double)value / Maximum) * 100);

        WindowsTerminal.SetProgressbar(ProgressbarState.Default, percent);
        
        int maxChars = Console.WindowWidth - 2;
        int chars = (int)Math.Ceiling(((double)value / Maximum) * maxChars);
        _builder
            .New()
            .Append('┌')
            .Append('─', maxChars)
            .AppendLine("┐")
            .Append('│');
        for (int i = 0; i < chars; i++)
        {
            _builder.WithForegroundColor(TerminalColor.Green).Append('█');
        }

        _builder
            .ResetFormat()
            .Append(' ', maxChars - chars)
            .AppendLine("│")
            .Append('└')
            .Append('─', maxChars)
            .AppendLine("┘");

        AnsiConsole.WriteLine(_builder.ToString());
    }
}
