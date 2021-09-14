//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Views;
using Terminal.Gui;

namespace BookGen.Gui
{
    internal class HelpView : ViewBase<IMainViewController>
    {
        private ListView? _listbox;
        private TextView? _text;

        public HelpView(IMainViewController controller, IConsoleUi consoleUi) : base(controller, consoleUi)
        {
        }

        public override void DrawView()
        {
            _listbox = AddListBox(new ListBoxElement
            {
                Width = 30.0f,
                SelectedIndex = 0,
                Title = "Navigation:",
                SelectedItemChanged = OnHelpChange
            });
            _listbox.SetSource(Controller.HelpItemSource);
            _text = AddTextView(new TextBoxElement
            {
                Width = 70.0f,
                IsReadonly = true,
            });

        }

        private void OnHelpChange(ListViewItemEventArgs obj)
        {
            Controller.SelectedHelpItemChange(obj.Item);
        }

        public override void Refresh()
        {
            if (_listbox != null)
                _listbox.SelectedItem = Controller.SelectedHelpIndex;
            if (_text != null)
                _text.Text = Controller.CurrentHelpText;
        }
    }
}
