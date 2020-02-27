//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.XmlEntities;
using System;
using System.IO;
using System.Xml.Serialization;
using Terminal.Gui;

namespace BookGen.Gui
{
    public class UiRunner
    {
        private UiElementFactory _elementFactory;

        public UiRunner()
        {
            Application.Init();
        }

        public void Run(Stream view, object model)
        {
            var binder = new Binder(model);
            _elementFactory = new UiElementFactory(binder);

            XWindow deserialized = DeserializeXmlView(view);
            Window window = ParseDeserialized(deserialized);

            Application.Top.Add(window);

            Application.Run();
        }

        private XWindow DeserializeXmlView(Stream view)
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

            int row = 1;

            foreach (var child in deserialized.Children)
            {
                View rendered = null;
                if (child is XSpacer spacer)
                {
                    row += spacer.Rows;
                }
                else if (child is XTextBlock textBlock)
                {
                    _elementFactory.RenderTextBlock(textBlock, root, ref row);
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

        private View RenderSimple(XView child, Window root, int row)
        {
            switch (child)
            {
                case XButton button:
                    return _elementFactory.CreateButton(button, root, row);
                case XLabel label:
                    return _elementFactory.CreateLabel(label, root, row);;
                default:
                    throw new InvalidOperationException($"Unknown node type: {child.GetType().Name}");
            }
        }
    }
}
