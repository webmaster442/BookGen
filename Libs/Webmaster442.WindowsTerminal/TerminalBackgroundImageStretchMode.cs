using System;
using System.Text.Json.Serialization;

namespace Webmaster442.WindowsTerminal;

/// <summary>
/// This sets how the background image is resized to fill the window.
/// </summary>
public enum TerminalBackgroundImageStretchMode
{
    None,
    Fill,
    Uniform,
    UniformToFill,
}
