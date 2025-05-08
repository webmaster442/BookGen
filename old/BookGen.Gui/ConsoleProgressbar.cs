//-----------------------------------------------------------------------------
// (c) 2021-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli.Mediator;

using Webmaster442.WindowsTerminal.Wigets;

using static BookGen.Gui.MessageTypes;

namespace BookGen.Gui;

public sealed class ConsoleProgressbar : Progressbar
{
    private readonly IMediator _mediator;

    public ConsoleProgressbar(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void OnShow()
    {
        _mediator.Notify(new BeginLogRedirectMessage());
        base.OnShow();
    }

    public override void OnHide()
    {
        _mediator.Notify(new EndLogRedirectMessage());
        base.OnHide();
    }
}
