// --------------------------------------------------------------------------
// Copyright (c) 2024-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
// --------------------------------------------------------------------------

namespace BookGen.Infrastructure.Terminal;

/// <summary>
/// Base class for wigets
/// </summary>
public abstract class WigetBase
{
    /// <summary>
    /// Gets if the wiget is currently shown.
    /// </summary>
    protected bool IsShowing { get; private set; }

    /// <summary>
    /// Gets if the wiget is currently shown in the alternate buffer.
    /// </summary>
    protected bool IsAlternateBuffer { get; private set; }

    /// <summary>
    /// Shows the wiget. If useAlternateBuffer is true, the wiget will be shown in the alternate buffer.
    /// </summary>
    /// <param name="useAlternateBuffer">id set to true the wiget will be shown in the alternate buffer.</param>
    public void Show(bool useAlternateBuffer)
    {
        IsShowing = true;
        IsAlternateBuffer = useAlternateBuffer;
        if (IsAlternateBuffer)
            Webmaster442.WindowsTerminal.Terminal.SwitchToAlternateBuffer();
        OnShow();
    }

    /// <summary>
    /// Hides the wiget
    /// </summary>
    public void Hide()
    {
        OnHide();
        if (IsAlternateBuffer)
            Webmaster442.WindowsTerminal.Terminal.SwitchToMainBuffer();
        IsShowing = false;
    }

    /// <summary>
    /// An overridable method that is called when the wiget is shown
    /// </summary>
    public abstract void OnShow();


    /// <summary>
    /// An overridable method that is called when the wiget is hidden
    /// </summary>
    public abstract void OnHide();
}
