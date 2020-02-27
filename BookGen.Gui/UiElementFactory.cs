//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.XmlEntities;
using Terminal.Gui;

namespace BookGen.Gui
{
    internal class UiElementFactory
    {
        private readonly Binder _binder;

        public UiElementFactory(Binder binder)
        {
            _binder = binder;
        }

        public Button CreateButton(XButton button, Window root, int row)
        {
            var result = new Button(button.Text ?? "")
            {
                X = Pos.Left(root) + button.Left,
                Y = Pos.Top(root) + row
            };
            if (button.Command != null)
                result.Clicked = _binder.InvokeCommand(button.Command);
            return result;
        }

        public Label CreateLabel(XLabel label, Window root, int row)
        {
            return new Label(label.Text ?? "")
            {
                X = Pos.Left(root) + label.Left,
                Y = Pos.Top(root) + row
            };
        }
    }
}
