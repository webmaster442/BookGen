﻿//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Mvvm;
using BookGen.Gui.XmlEntities;
using System.Diagnostics;
using System.Xml.Serialization;
using Terminal.Gui;

namespace BookGen.Gui
{
    public sealed class ConsoleUi : IView, IDisposable
    {
        private Window? _window;
        private Binder? _binder;
        private bool _running;

        public void Run(Stream view, ViewModelBase model)
        {
#pragma warning disable S2696 // Instance members should not write to "static" fields
            Application.UseSystemConsole = false;
#pragma warning restore S2696 // Instance members should not write to "static" fields
            _binder = new Binder(model);
            XWindow? deserialized = DeserializeXmlView(view);
            if (deserialized != null)
            {
                if (Application.Top == null)
                    Application.Init();

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
            if (OnNavigaton != null)
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
                    SuspendUi();
                    Run(result.Value.view, result.Value.model);
                }

            }
        }


        public void SuspendUi()
        {
            if (Application.Top?.Running == true)
            {
                Application.RequestStop();
                Application.Top.Remove(_window);
                Application.Shutdown();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
                _running = false;
            }
        }

        public void ResumeUi()
        {
            if (Application.Top == null)
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

                if (Application.Top?.Subviews.Count < 1)
                {
                    Application.Top.Add(_window);
                }
            }
            Application.Run(OnError);
            _running = true;
        }

        private bool OnError(Exception arg)
        {
            Debug.WriteLine(arg.Message);
            return false;
        }

        public void ExitApp()
        {
            Application.Driver.End();
            Application.Current.RequestStop();
        }

        public void UpdateBindingsToModel()
        {
            _binder?.UpdateToModel();
        }

        public void UpdateViewFromModel()
        {
            _binder?.UpdateFromModel();
        }

        private XWindow? DeserializeXmlView(Stream view)
        {
            var xs = new XmlSerializer(typeof(XWindow));
            return xs.Deserialize(view) as XWindow;
        }

        public void Dispose()
        {
            if (_window != null && _running)
            {
                _window.Dispose();
                _window = null;
            }
        }
    }
}