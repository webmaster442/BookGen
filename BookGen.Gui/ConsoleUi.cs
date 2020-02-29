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

        public ConsoleUi()
        {
            Application.Init();
        }

        public void Run(Stream view, ViewModelBase model)
        {
            var binder = new Binder(model);
            _elementFactory = new UiElementFactory(binder);

            XWindow deserialized = DeserializeXmlView(view);
            Window window = ParseDeserialized(deserialized);
            model.InjectView(this);

            Application.Top.Add(window);

            Application.Run();
        }

        public void SuspendUi()
        {
            Application.Top.Running = false;
        }

        public void ResumeUi()
        {
            Application.Run();
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
                default:
                    throw new InvalidOperationException($"Unknown node type: {child.GetType().Name}");
            }
        }
    }
}
