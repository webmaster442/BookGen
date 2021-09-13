//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


using BookGen.Gui.Views;
using System;
using Terminal.Gui;

namespace BookGen.Gui
{
    public sealed class ConsoleUi : IConsoleUi, IDisposable
    {
        private ViewBase? _currentWindow;

        private void RunView(ViewBase view)
        {
#pragma warning disable S2696 // Instance members should not write to "static" fields
            Application.UseSystemConsole = false;
#pragma warning restore S2696 // Instance members should not write to "static" fields
            view.DrawView();
            _currentWindow = view;

            ResumeUi();
        }

        public void RunMainView(IMainViewController controller)
        {
            var view = new MainGuiView(controller, this);
            RunView(view);
        }

        public void SuspendUi()
        {
            if (Application.Top?.Running == true)
            {
                Application.RequestStop();
                Application.Driver.End();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
            }
        }

        public void ResumeUi()
        {
            Application.Init();
            if (_currentWindow != null)
            {
                _currentWindow.ColorScheme = new ColorScheme
                {
                    Focus = Terminal.Gui.Attribute.Make(Color.Gray, Color.Blue),
                    HotFocus = Terminal.Gui.Attribute.Make(Color.Gray, Color.Black),
                    HotNormal = Terminal.Gui.Attribute.Make(Color.Gray, Color.Black),
                    Normal = Terminal.Gui.Attribute.Make(Color.Gray, Color.Black),
                };
                Application.Top.Add(_currentWindow);
            }
            Application.Run();
        }

        public void ExitApp()
        {
            SuspendUi();
            Dispose();
            Environment.Exit(0);
        }

        public void Dispose()
        {
            if (_currentWindow != null)
            {
                _currentWindow.Dispose();
                _currentWindow = null;
            }
        }
    }
}
