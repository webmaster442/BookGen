﻿//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;

using BookGen.Api;
using BookGen.DomainServices;
using BookGen.Interfaces;

namespace BookGen.AssemblyDocumenter;

public static class XmlDocValidator
{
    public static bool ValidateXml(FsPath xml, ILog log)
    {
        int errors = 0;
        try
        {
            var schema = new XmlSchemaSet();
            schema.Add(LoadXsd());
            using (FileStream? stream = xml.OpenStreamRead())
            {
                using (var rd = XmlReader.Create(stream))
                {
                    var doc = XDocument.Load(rd);
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
        Assembly current = typeof(XmlDocValidator).Assembly;
        using (System.IO.Stream? stream = current.GetManifestResourceStream("BookGen.AssemblyDocumenter.DocComment.xsd"))
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
