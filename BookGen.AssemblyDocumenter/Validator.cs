//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using System;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace BookGen.AssemblyDocumenter
{
    public static class Validator
    {
        public static bool ValidateXml(FsPath xml, ILog log)
        {
            int errors = 0;
            try
            {
                XmlSchemaSet schema = new XmlSchemaSet();
                schema.Add(LoadXsd());
                using (var stream = xml.OpenStream())
                {
                    using (XmlReader rd = XmlReader.Create(stream))
                    {
                        XDocument doc = XDocument.Load(rd);
                        doc.Validate(schema, (s, e) =>
                        {
                            if (e.Severity == XmlSeverityType.Error)
                            {
                                log.Warning(e.Message);
                                ++errors;
                            }
                        });
                    }
                }
                return errors == 0;
            }
            catch (Exception ex)
            {
                log.Critical(ex);
                return false;
            }
        }

        private static XmlSchema LoadXsd()
        {
            Assembly current = typeof(Validator).Assembly;
            using (var stream = current.GetManifestResourceStream("BookGen.AssemblyDocumenter.DocComment.xsd"))
            {
                if (stream != null)
                {
                    var schema = XmlSchema.Read(stream, null);
                    if (schema != null)
                        return schema;
                }
                throw new InvalidOperationException();
            }
        }
    }
}
