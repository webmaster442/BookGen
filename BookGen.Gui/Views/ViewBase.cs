//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using Terminal.Gui;

namespace BookGen.Gui.Views
{
#pragma warning disable S3442 // "abstract" classes should not have "public" constructors
    internal abstract class ViewBase<TController>
        : ViewBase where TController : IControllerBase
    {
        public ViewBase(TController controller, IConsoleUi consoleUi)

        {
            Controller = controller;
            Controller.Ui = consoleUi;
            Ui = consoleUi;
        }

        protected TController Controller { get; }
    }

    internal abstract class ViewBase : Window
    {
        public abstract void DrawView();

        protected IConsoleUi? Ui { get; set; }

        private static void SetWidth(View view, ElementBase element)
        {
            if (element.WidthHandling == WidthHandling.Auto
                || float.IsNaN(element.Width))
            {
                return;
            }

            switch (element.WidthHandling)
            {
                case WidthHandling.Percent:
                    view.Width = Dim.Percent(element.Width);
                    break;
                case WidthHandling.Columns:
                    view.Width = (int)Math.Ceiling(element.Width);
                    break;
                default:
                    return;
            }
        }

        protected void AddButton(ButtonElement element)
        {
            var result = new Button(element.Text)
            {
                X = Pos.Left(this) + element.Left,
                Y = Pos.Top(this) + element.Top,
            };
            if (element.OnClick != null)
            {
                if (element.ClickSuspendsUi)
                {
                    result.Clicked += () =>
                    {
                        Ui?.SuspendUi();
                        element.OnClick();
                        Console.WriteLine("Press a key to continue...");
                        Console.ReadKey();
                        Ui?.ResumeUi();
                    };
                }
                else
                {
                    result.Clicked += element.OnClick;
                }    
            }

            SetWidth(result, element);
            Add(result);
        }

        protected void AddText(TextElement element)
        {
            var lines = element.Text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 1) return;
            int rowCounter = element.Top;
            foreach (var line in lines)
            {
                var label = new Label(line)
                {
                    X = Pos.Left(this) + element.Left,
                    Y = Pos.Top(this) + rowCounter,
                };
                SetWidth(label, element);
                Add(label);
                ++rowCounter;
            }
        }

        protected void AddCheckBox(CheckBoxElement element)
        {
            var result = new CheckBox(element.Text)
            {
                X = Pos.Left(this) + element.Left,
                Y = Pos.Top(this) + element.Top
            };
            if (element.OnCheckedChange != null)
                result.Toggled += element.OnCheckedChange;
        }
    }
#pragma warning restore S3442 // "abstract" classes should not have "public" constructors
}
