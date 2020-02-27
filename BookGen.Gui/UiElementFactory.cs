//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.XmlEntities;
using Terminal.Gui;

namespace BookGen.Gui
{
    internal static class UiElementFactory
    {
        public static Button CreateButton(XButton button, Window root, int row)
        {
            return new Button(button.Text ?? "")
            {
                X = Pos.Left(root) + button.Left,
                Y = Pos.Top(root) + row
            };
        }

        public static Label CreateLabel(XLabel label, Window root, int row)
        {
            return new Label(label.Text ?? "")
            {
                X = Pos.Left(root) + label.Left,
                Y = Pos.Top(root) + row
            };
        }
    }
}
