//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.XmlEntities;
using System;
using Terminal.Gui;

namespace BookGen.Gui
{
    internal class SplitView : View
    {
        private Pos _left = 0;
        private readonly Binder _binder;

        public SplitView(XSPlitView xSPlitView, Binder binder)
        {
            _binder = binder;
            Render(xSPlitView);
            Width = Dim.Fill();
            Y = xSPlitView.Top;
            Height = Dim.Fill() - xSPlitView.Top;
        }

        private void Render(XSPlitView xSPlitView)
        {
            foreach (var child in xSPlitView.Children)
            {
                switch (child)
                {
                    case XListBox listBox:
                        RenderListBox(listBox);
                        break;
                    case XTextBox textBox:
                        RenderTextBox(textBox);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        private void RenderTextBox(XTextBox textBox)
        {
            var result = new FrameView()
            {
                X = Pos.Left(this) + textBox.Left + _left,
            };

            var text = new TextView
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ReadOnly = textBox.IsReadonly
            };

            if (Binder.IsBindable(textBox.Text))
            {
                text.Text = _binder.GetBindedText(textBox.Text);
                _binder.Register(textBox, text, typeof(string));
            }
            else
            {
                text.Text = textBox.Text;
            }

            result.Add(text);
            result.Height = Dim.Fill();
            UiPage.SetWidth(result, textBox);

            Add(result);
            _left += Pos.Right(result);
        }

        private void RenderListBox(XListBox listBox)
        {
            var result = new FrameView()
            {
                Title = listBox.Title,
                X = Pos.Left(this) + listBox.Left + _left,
            };

            var list = new ListView
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };

            list.SelectedItemChanged += List_SelectedItemChanged;

            if (Binder.IsBindable(listBox.ItemSourceProperty))
            {
                list.SetSource(_binder.GetBindedList(listBox.ItemSourceProperty));
            }
            if (Binder.IsBindable(listBox.SelectedIndex))
            {
                list.SelectedItem = Convert.ToInt32(_binder.GetBindedText(listBox.SelectedIndex));
                _binder.Register(listBox, list, typeof(int));
            }

            result.Add(list);
            result.Height = Dim.Fill();
            UiPage.SetWidth(result, listBox);

            Add(result);
            _left += Pos.Right(result);
        }

        private void List_SelectedItemChanged(ListViewItemEventArgs obj)
        {
            _binder.UpdateToModel();
        }
    }
}
