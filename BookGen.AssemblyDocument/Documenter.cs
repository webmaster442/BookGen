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
        private int _unknown;

        public Documenter(ILog log)
        {
            _log = log;
            _loader = new XmlLoader();
            _unknown = 0;
        }

        private static IEnumerable<Type> GetDocumentableTypes(FsPath assembly)
        {
            var asm = Assembly.LoadFrom(assembly.ToString());
            return asm.GetExportedTypes();
        }

        private string CreateTypeKey(Type type)
        {
            if (string.IsNullOrEmpty(type.FullName))
            {
                return $"Unknown type #{++_unknown}";
            }
            return type.FullName;
        }

        public IDictionary<string, string> DocumentAssembly(FsPath assembly, FsPath xmlDoc)
        {
            var results = new Dictionary<string, string>();

            if (!_loader.TryValidatedLoad(xmlDoc.ToString(), out XmlDoc.Doc? doc)
                || doc == null)
            {
                _log.Critical("Xml file can't be loaded or not a valid XML documentation");
                return results;
            }

            var documenters = new DocumenterBase[]
            {
                new TypeHeaderDocumenter(doc, _log),
                new EnumValuesDocumenter(doc, _log),
                new TypeMembersDocumenter(doc, _log),
            };
            try
            {
                foreach (var type in GetDocumentableTypes(assembly))
                {
                    var typeDoc = new MarkdownBuilder();
                    foreach (var documenter in documenters)
                    {
                        if (documenter.CanExecute(type))
                            documenter.Execute(type, typeDoc);
                    }
                    results.Add(CreateTypeKey(type), typeDoc.ToString());
                }
                return results;
            }
            catch (Exception ex)
            {
                _log.Critical(ex);
                return new Dictionary<string, string>();
            }
        }
    }
}
