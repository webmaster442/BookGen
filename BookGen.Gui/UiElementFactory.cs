﻿//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Ui.XmlEntities;
using System;
using System.Collections.Generic;
using Terminal.Gui;

namespace BookGen.Ui
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
            {
                var act = _binder.BindCommand(button.Command);
                result.Clicked += act;
            }
            return result;
        }

        public Label CreateLabel(XLabel label, Window root, int row)
        {
            string text = label.Text ?? "";

            if (Binder.IsBindable(text))
                text = _binder.GetBindedText(text);

            return new Label(text)
            {
                X = Pos.Left(root) + label.Left,
                Y = Pos.Top(root) + row
            };
        }

        public CheckBox CreateCheckBox(XCheckBox checkBox, Window root, int row)
        {
            string text = checkBox.Text ?? "";

            if (Binder.IsBindable(text))
                text = _binder.GetBindedText(text);
                
            var result = new CheckBox(text)
            {
                X = Pos.Left(root) + checkBox.Left,
                Y = Pos.Top(root) + row,
            };

            if (Binder.IsBindable(checkBox.IsChecked))
            {
                result.Checked = _binder.GetBindedBool(checkBox.IsChecked);
                _binder.Register(checkBox, result);
            }

            return result;
        }

        internal void RenderTextBlock(XTextBlock textBlock,
                                      Window root, 
                                      List<IDisposable> disposables,
                                      ref int row)
        {
            var lines = textBlock.Text?.ToString().Split('\n');
            if (lines == null) return;
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                
                var label = new Label(line)
                {
                    X = Pos.Left(root) + textBlock.Left,
                    Y = Pos.Top(root) + row,
                };
                disposables.Add(label);
                root.Add(label);
                ++row;
            }

        }
    }
}
