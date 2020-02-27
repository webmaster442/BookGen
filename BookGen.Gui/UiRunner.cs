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
            SerializeXml();
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

        public void SerializeXml()
        {
            XmlSerializer xs = new XmlSerializer(typeof(XWindow));

            var obj = new XWindow
            {
                Children = new System.Collections.Generic.List<XView>
                {
                    new XButton(),
                    new XLabel()
                }
            };

            StringWriter sw = new StringWriter();
            xs.Serialize(sw, obj);
            string content = sw.ToString();
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
            switch (child)
            {
                case XButton button:
                    return UiElementFactory.CreateButton(button, root, row);
                case XLabel label:
                    return UiElementFactory.CreateLabel(label, root, row);
                default:
                    return null;
            }
        }
    }
}
