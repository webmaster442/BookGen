using BookGen.Gui.XmlEntities;
using System;
using System.IO;
using System.Xml.Serialization;
using Terminal.Gui;

namespace BookGen.Gui
{
    public class UiRunner
    {
        public UiRunner()
        {
            Application.Init();
        }

        public void Run(Stream view)
        {
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
                if (child is XSpacer spacer)
                {
                    row += spacer.Rows;
                }
                else
                {
                    View rendered = Render(child, root, row);
                    root.Add(rendered);
                    ++row;
                }
            }

            return root;
        }

        private View Render(XView child, Window root, int row)
        {
            switch (child)
            {
                case XButton button:
                    return UiElementFactory.CreateButton(button, root, row);
                case XLabel label:
                    return UiElementFactory.CreateLabel(label, root, row);
                default:
                    throw new InvalidOperationException($"Unknown node type: {child.GetType().Name}");
            }
        }
    }
}
