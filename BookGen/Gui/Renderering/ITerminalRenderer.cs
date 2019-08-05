//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Gui.Renderering
{
    /// <summary>
    /// Terminal Renderer base
    /// </summary>
    internal interface ITerminalRenderer
    {
        /// <summary>
        /// Renderes a piece of text
        /// </summary>
        /// <param name="text">Text to display. Can be a format string</param>
        /// <param name="foreground">Foreground color</param>
        /// <param name="background">Background color</param>
        /// <param name="format">Text format options</param>
        /// <param name="arguments">Additional arguments, that will be inserted into the format string</param>
        void Text(string text,
                  Color foreground,
                  Color background,
                  TextFormat format,
                  params object[] arguments);

        /// <summary>
        /// Display an error
        /// </summary>
        /// <param name="msg">Error message</param>
        void DisplayError(string msg);

        /// <summary>
        /// Renderes the "Pres a key to continue..." message and waits for a key input
        /// </summary>
        void PressKeyContinue();

        /// <summary>
        /// Renderes a newline
        /// </summary>
        void NewLine();

        /// <summary>
        /// Clears the Screen
        /// </summary>
        void Clear();

        /// <summary>
        /// Sets the window title
        /// </summary>
        /// <param name="title">Terminal window title</param>
        void SetWindowTitle(string title);

        /// <summary>
        /// Read a numeric choice
        /// </summary>
        /// <returns>Numeric choice. if parsing was not successfull null is returned</returns>
        int? GetInputChoice();

        /// <summary>
        /// Read a single character from input
        /// </summary>
        /// <returns>A single char from the input</returns>
        char ReadChar();
    }
}
