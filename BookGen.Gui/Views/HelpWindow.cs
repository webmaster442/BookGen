using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace BookGen.Gui.Views
{
    internal class HelpWindow : View
    {
        private readonly IMainViewController _controller;

        public HelpWindow(IMainViewController controller)
        {
            _controller = controller;
        }

        public void DrawView()
        {
            var frame = new FrameView()
            {
                X = Pos.Left(this),
                Width = Dim.Percent(70),
                Height = Dim.Fill(),
            };
            var text = new TextView
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ReadOnly = true,
                Text = _controller.CurrentHelpText,
            };
            frame.Add(text);
            Add(frame);

            var listFrame = new FrameView()
            {
                Title = "Navigation:",
                X = Pos.Left(this),
                Width = Dim.Percent(70),
            };
            var list = new ListView
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };
            list.SelectedItemChanged += List_SelectedItemChanged;
            list.SetSource(_controller.HelpItemSource);
            listFrame.Add(list);
            Add(listFrame);
        }

        private void List_SelectedItemChanged(ListViewItemEventArgs obj)
        {
            _controller.SelectedHelpIndex = obj.Item;
        }
    }
}