//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.XmlEntities;
using NStack;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terminal.Gui;

namespace BookGen.Gui
{
    internal class UiPage : Window
    {
        private readonly Binder _binder;

        public UiPage(XWindow window, Binder binder)
        {
            _binder = binder;
            Render(window);
        }

        public static void SetWidth(View view, XView xView)
        {
            if (xView.WidthHandling == WidthHandling.Auto
                || float.IsNaN(xView.Width))
            {
                return;
            }

            switch (xView.WidthHandling)
            {
                case WidthHandling.Percent:
                    view.Width = Dim.Percent(xView.Width);
                    break;
                case WidthHandling.Columns:
                    view.Width = (int)Math.Ceiling(xView.Width);
                    break;
                default:
                    return;
            }
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
                    case XTextBlock textBlock:
                        RenderTextBlock(textBlock);
                        break;
                    case XButton button:
                        RenderButton(button);
                        break;
                    case XLabel label:
                        RenderLabel(label);
                        break;
                    case XRadioGroup radioGroup:
                        RenderRadioButton(radioGroup);
                        break;
                    case XCheckBox checkBox:
                        RenderCheckBox(checkBox);
                        break;
                    case XSPlitView sPlitView:
                        RenderSplitView(sPlitView);
                        break;
                    case XVerticalLine verticalLine:
                        RenderVerticalLine(verticalLine);
                        break;
                    default:
                        throw new InvalidOperationException($"Unknown node type: {child.GetType().Name}");
                }
            }
        }

        private void RenderVerticalLine(XVerticalLine verticalLine)
        {
            int end = verticalLine.Height + verticalLine.Top;

            for (int i = verticalLine.Top; i < end; i++)
            {
                var label = new Label(verticalLine.Symbol.ToString())
                {
                    X = Pos.Left(this) + verticalLine.Left,
                    Y = Pos.Top(this) + i,
                };
                SetWidth(label, verticalLine);
                Add(label);
            }
        }

        private void RenderSplitView(XSPlitView sPlitView)
        {
            Add(new SplitView(sPlitView, _binder));
        }

        private void RenderButton(XButton button)
        {
            var result = new Button(button.Text ?? "")
            {
                X = Pos.Left(this) + button.Left,
                Y = Pos.Top(this) + button.Top,
                Data = _binder.BindCommand(button.Command)
            };
            result.Clicked += () =>
            {
                _binder.TryInvokeCommand(result.Data as string);
            };
            SetWidth(result, button);
            Add(result);
        }

        private void RenderLabel(XLabel label)
        {
            string text = label.Text ?? "";

            if (Binder.IsBindable(text))
                text = _binder.GetBindedText(text);


            var result = new Label(text)
            {
                X = Pos.Left(this) + label.Left,
                Y = Pos.Top(this) + label.Top,
            };
            SetWidth(result, label);
            Add(result);
        }

        private void RenderTextBlock(XTextBlock textBlock)
        {
            var lines = textBlock.Text?.ToString().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines == null) return;
            int row = textBlock.Top;
            foreach (var line in lines)
            {
                var label = new Label(line)
                {
                    X = Pos.Left(this) + textBlock.Left,
                    Y = Pos.Top(this) + row,
                };
                SetWidth(label, textBlock);
                Add(label);
                ++row;
            }
        }

        private void RenderCheckBox(XCheckBox checkBox)
        {
            string text = checkBox.Text ?? "";

            if (Binder.IsBindable(text))
                text = _binder.GetBindedText(text);

            var result = new CheckBox(text)
            {
                X = Pos.Left(this) + checkBox.Left,
                Y = Pos.Top(this) + checkBox.Top,
            };

            if (Binder.IsBindable(checkBox.IsChecked))
            {
                result.Checked = _binder.GetBindedBool(checkBox.IsChecked);
                _binder.Register(checkBox, result, typeof(bool));
            }
            SetWidth(result, checkBox);
            Add(result);
        }


        private void RenderRadioButton(XRadioGroup radioGroup)
        {
            var result = new RadioGroup
            {
                RadioLabels = ConvertTexts(radioGroup.Options),
                X = Pos.Left(this) + radioGroup.Left,
                Y = Pos.Top(this) + radioGroup.Top,
            };

            if (Binder.IsBindable(radioGroup.SelectedIndex))
            {
                result.SelectedItem = Convert.ToInt32(_binder.GetBindedText(radioGroup.SelectedIndex));
                _binder.Register(radioGroup, result, typeof(int));
            }
            SetWidth(result, radioGroup);
            Add(result);
        }

        private static ustring[] ConvertTexts(string[] texts)
        {
            var result = new ustring[texts.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = texts[i];
            }
            return result;
        }
    }
}
