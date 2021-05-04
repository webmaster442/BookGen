//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Ui.Mvvm;
using BookGen.Ui.XmlEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Terminal.Gui;

namespace BookGen.Ui
{
    public sealed class ConsoleUi: IView, IDisposable
    {
        private UiElementFactory? _elementFactory;
        private Window? _window;
        private Binder? _binder;
        private readonly List<IDisposable> _disposables;

        public ConsoleUi()
        {
            _disposables = new List<IDisposable>();
        }

        public void Run(Stream view, ViewModelBase model)
        {
            Application.UseSystemConsole = false;
            _binder = new Binder(model);
            _elementFactory = new UiElementFactory(_binder);
            XWindow? deserialized = DeserializeXmlView(view);
            if (deserialized != null)
            {
                _window = ParseDeserialized(deserialized);
                model.InjectView(this);
                ResumeUi();
            }
            else
            {
                throw new InvalidOperationException("View load error");
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

        private Window ParseDeserialized(XWindow deserialized)
        {
            Window root = new Window(deserialized.Title ?? "")
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            _disposables.Add(root);

            int row = 1;

            foreach (var child in deserialized.Children)
            {
                View? rendered = null;
                if (child is XSpacer spacer)
                {
                    row += spacer.Rows;
                }
                else if (child is XTextBlock textBlock)
                {
                    _elementFactory?.RenderTextBlock(textBlock, root, _disposables, ref row);
                }
                else
                {
                    rendered = RenderSimple(child, root, row);
                    if (rendered != null)
                    {
                        _disposables.Add(rendered);
                        root.Add(rendered);
                        ++row;
                    }
                }
            }

            return root;
        }

        private View? RenderSimple(XView child, Window root, int row)
        {
            switch (child)
            {
                case XButton button:
                    return _elementFactory?.CreateButton(button, root, row);
                case XLabel label:
                    return _elementFactory?.CreateLabel(label, root, row);
                case XCheckBox checkBox:
                    return _elementFactory?.CreateCheckBox(checkBox, root, row);
                default:
                    throw new InvalidOperationException($"Unknown node type: {child.GetType().Name}");
            }
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}
