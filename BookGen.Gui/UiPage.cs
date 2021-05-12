﻿//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Ui.XmlEntities;
using System;
using Terminal.Gui;

namespace BookGen.Ui
{
    internal class UiPage : Window
    {
        private readonly Binder _binder;
        private int _rowCounter;

        public UiPage(XWindow window, Binder binder)
        {
            _rowCounter = 0;
            _binder = binder;
            Render(window);
        }

        private void Render(XWindow window)
        {
            Width = Dim.Fill();
            Height = Dim.Fill();
            Title = window.Title;

            foreach (var child in window.Children)
            {
                switch (child)
                {
                    case XSpacer spacer:
                        _rowCounter += spacer.Rows;
                        continue;
                    case XTextBlock textBlock:
                        RenderTextBlock(textBlock);
                        break;
                    case XButton button:
                        RenderButton(button);
                        break;
                    case XLabel label:
                        RenderLabel(label);
                        break;
                    case XCheckBox checkBox:
                        RenderCheckBox(checkBox);
                        break;
                    default:
                        throw new InvalidOperationException($"Unknown node type: {child.GetType().Name}");
                }
                ++_rowCounter;
            }
        }

        private void RenderButton(XButton button)
        {
            var result = new Button(button.Text ?? "")
            {
                X = Pos.Left(this) + button.Left,
                Y = Pos.Top(this) + _rowCounter,
            };
            if (button.Command != null)
            {
                var act = _binder.BindCommand(button.Command);
                result.Clicked += act;
            }
            Add(result);
        }

        private void RenderLabel(XLabel label)
        {
            string text = label.Text ?? "";

            if (Binder.IsBindable(text))
                text = _binder.GetBindedText(text);

            Add(new Label(text)
            {
                X = Pos.Left(this) + label.Left,
                Y = Pos.Top(this) + _rowCounter,
            });
        }

        private void RenderTextBlock(XTextBlock textBlock)
        {
            var lines = textBlock.Text?.ToString().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines == null) return;
            int row = _rowCounter;
            foreach (var line in lines)
            {
                var label = new Label(line)
                {
                    X = Pos.Left(this) + textBlock.Left,
                    Y = Pos.Top(this) + row,
                };
                Add(label);
                ++row;
            }
            _rowCounter = row;
        }

        private void RenderCheckBox(XCheckBox checkBox)
        {
            string text = checkBox.Text ?? "";

            if (Binder.IsBindable(text))
                text = _binder.GetBindedText(text);

            var result = new CheckBox(text)
            {
                X = Pos.Left(this) + checkBox.Left,
                Y = Pos.Top(this) + _rowCounter,
            };

            if (Binder.IsBindable(checkBox.IsChecked))
            {
                result.Checked = _binder.GetBindedBool(checkBox.IsChecked);
                _binder.Register(checkBox, result);
            }

            Add(result);
        }

    }
}
