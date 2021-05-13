//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Ui.Mvvm;
using BookGen.Ui.XmlEntities;
using System;
using System.IO;
using System.Xml.Serialization;
using Terminal.Gui;

namespace BookGen.Ui
{
    public sealed class ConsoleUi: IView, IDisposable
    {
        private Window? _window;
        private Binder? _binder;

        public void Run(Stream view, ViewModelBase model)
        {
#pragma warning disable S2696 // Instance members should not write to "static" fields
            Application.UseSystemConsole = false;
#pragma warning restore S2696 // Instance members should not write to "static" fields
            _binder = new Binder(model);
            XWindow? deserialized = DeserializeXmlView(view);
            if (deserialized != null)
            {
                _window = new UiPage(deserialized, _binder);
                model.InjectView(this);
                ResumeUi();
            }
            else
            {
                throw new InvalidOperationException("View load error");
            }
        }

        public event Func<string, (Stream view, ViewModelBase model)>? OnNavigaton;

        public void SwitchToView(string name)
        {
            if (OnNavigaton!= null)
            {
                (Stream view, ViewModelBase model)? result = OnNavigaton?.Invoke(name);
                if (_window != null)
                {
                    _window.Dispose();
                    _binder = null;
                    _window = null;
                }
                if (result != null)
                {
                    Run(result.Value.view, result.Value.model);
                }

            }
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
            if (_window != null)
            {
                _window.ColorScheme = new ColorScheme
                {
                    Focus = Terminal.Gui.Attribute.Make(Color.Gray, Color.Blue),
                    HotFocus = Terminal.Gui.Attribute.Make(Color.Gray, Color.Black),
                    HotNormal = Terminal.Gui.Attribute.Make(Color.Gray, Color.Black),
                    Normal = Terminal.Gui.Attribute.Make(Color.Gray, Color.Black),
                };
                Application.Top.Add(_window);
            }
            Application.Run();
        }

        public void ExitApp()
        {
            SuspendUi();
            Dispose();
            Environment.Exit(0);
        }

        public void UpdateBindingsToModel()
        {
            _binder?.Update();
        }

        private XWindow? DeserializeXmlView(Stream view)
        {
            XmlSerializer xs = new XmlSerializer(typeof(XWindow));
            return xs.Deserialize(view) as XWindow;
        }

        public void Dispose()
        {
            if (_window != null)
            {
                _window.Dispose();
                _window = null;
            }
        }
    }
}
