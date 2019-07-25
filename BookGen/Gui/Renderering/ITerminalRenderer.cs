//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Gui.Renderering
{
    internal interface ITerminalRenderer
    {
        void Text(string text,
                  Color foreground,
                  Color background,
                  TextFormat format,
                  params object[] arguments);

        void DisplayError(string msg);

        void PressKeyContinue();

        void NewLine();

        void Clear();

        void SetWindowTitle(string title);

        int GetInputChoice();
    }
}
