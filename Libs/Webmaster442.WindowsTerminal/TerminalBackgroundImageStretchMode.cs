namespace Webmaster442.WindowsTerminal;

/// <summary>
/// This sets how the background image is resized to fill the window.
/// </summary>
public enum TerminalBackgroundImageStretchMode
{
    /// <summary>
    /// The image is not resized.
    /// </summary>
    None,
    /// <summary>
    /// The image is resized to fill the destination dimensions. The aspect ratio is not preserved.
    /// </summary>
    Fill,
    /// <summary>
    /// The image is resized to fit in the destination dimensions while it preserves its native aspect ratio.
    /// </summary>
    Uniform,
    /// <summary>
    /// The image is resized to fill the destination dimensions while it preserves its native aspect ratio.
    /// If the aspect ratio of the destination rectangle differs from the source,
    /// the source content is clipped to fit in the destination dimensions.
    /// </summary>
    UniformToFill,
}
