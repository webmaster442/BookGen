using BookGen.Gui.XmlEntities;
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
                View rendered = Render(child, root, row);
                root.Add(rendered);
                ++row;
            }

            return root;
        }

        private View Render(XView child, Window root, int row)
        {
            if (child is XButton button)
            {
                return new Button(button.Text)
                {
                    X = Pos.Left(root) + button.Left,
                    Y = Pos.Top(root) + row
                };
            }
            else if (child is XLabel label)
            {
                return new Label(label.Text)
                {
                    X = Pos.Left(root) + label.Left,
                    Y = Pos.Top(root) + row
                };
            }
            else
                return null;
        }
    }
}
