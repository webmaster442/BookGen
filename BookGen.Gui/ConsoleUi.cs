//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Mvvm;
using BookGen.Gui.XmlEntities;
using System;
using System.IO;
using System.Xml.Serialization;
using Terminal.Gui;

namespace BookGen.Gui
{
    public class ConsoleUi: IView
    {
        private UiElementFactory? _elementFactory;
        private Window? _window;
        private Binder? _binder;

        public void Run(Stream view, ViewModelBase model)
        {
            Application.UseSystemConsole = true;
            _binder = new Binder(model);
            _elementFactory = new UiElementFactory(_binder);
            XWindow deserialized = DeserializeXmlView(view);
            _window = ParseDeserialized(deserialized);
            model.InjectView(this);
            ResumeUi();
        }

        public void SuspendUi()
        {
            if (Application.Top?.Running == true)
            {
                Application.RequestStop();
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.Clear();
        }

        public void ResumeUi()
        {
            Console.Clear();
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
            Environment.Exit(0);
        }

        public void UpdateBindingsToModel()
        {
            _binder?.Update();
        }

        private XWindow DeserializeXmlView(Stream view)
        {
            XmlSerializer xs = new XmlSerializer(typeof(XWindow));
            return (XWindow)xs.Deserialize(view);
        }

        private Window ParseDeserialized(XWindow deserialized)
        {
            Window root = new Window(deserialized.Title ?? "")
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

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
                    _elementFactory?.RenderTextBlock(textBlock, root, ref row);
                }
                else
                {
                    rendered = RenderSimple(child, root, row);
                    root.Add(rendered);
                    ++row;
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
    }
}
