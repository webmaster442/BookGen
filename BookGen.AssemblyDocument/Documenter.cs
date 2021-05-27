using BookGen.Api;
using BookGen.AssemblyDocument.Documenters;
using BookGen.Core;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BookGen.AssemblyDocument
{
    public class Documenter
    {
        private readonly ILog _log;
        private readonly XmlLoader _loader;

        public Documenter(ILog log)
        {
            _log = log;
            _loader = new XmlLoader();
        }

        private static IEnumerable<Type> GetDocumentableTypes(FsPath assembly)
        {
            var asm = Assembly.LoadFrom(assembly.ToString());
            return asm.GetExportedTypes();
        }

        public void DocumentAssembly(FsPath assembly, FsPath xmlDoc)
        {
            if (!_loader.TryValidatedLoad(xmlDoc.Filename, out XmlDoc.Doc? doc)
                || doc == null)
            {
                _log.Critical("Xml file can't be loaded or not a valid XML documentation");
                return;
            }

            var documenters = new DocumenterBase[]
            {
                new TypeHeaderDocumenter(doc, _log),

            };
            try
            {
                foreach (var type in GetDocumentableTypes(assembly))
                {
                    var typeDoc = new MarkdownBuilder();
                    foreach (var documenter in documenters)
                    {
                        documenter.Execute(type, typeDoc);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Critical(ex);
            }
        }
    }
}
