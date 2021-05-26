using BookGen.AssemblyDocument.XmlDoc;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BookGen.AssemblyDocument
{
    public class XmlLoader
    {
        private readonly XmlSchemaSet _validators;

        public XmlLoader()
        {
            var xsd = Resources.ResourceHandler.GetResourceFile<XmlLoader>("Schema.xsd");
            _validators = new XmlSchemaSet();
            _validators.Add("", XmlReader.Create(new StringReader(xsd)));
        }

        public bool TryValidatedLoad(string file, out Doc? document)
        {
            XDocument doc = XDocument.Load(file);
            document = null;
            bool valid = true;
            doc.Validate(_validators, (o, e) =>
            {
                valid = false;
            });

            if (valid)
            {
                XmlSerializer xs = new XmlSerializer(typeof(Doc));
                using (var f = File.OpenRead(file))
                {
                    document = xs.Deserialize(f) as Doc;
                }
            }

            return valid;
        }
    }
}
