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
