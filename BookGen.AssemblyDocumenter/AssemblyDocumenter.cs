using BookGen.Api;
using BookGen.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace BookGen.AssemblyDocumenter
{
    public class AssemblyDocumenter
    {
        private readonly ILog _log;

        public AssemblyDocumenter(ILog log)
        {
            _log = log;
        }

        private IEnumerable<Type> GetDocumentableTypes(FsPath assembly)
        {
            var asm = Assembly.LoadFrom(assembly.ToString());
            return asm.GetExportedTypes();
        }

        private void CreateTitle(MarkdownDocument document, Type type)
        {
            string typeInfo = "Class";
            string typeName = type.Name;
            if (type.IsInterface)
            {
                typeInfo = "Interface";
            }
            else if (type.IsEnum)
            {
                typeInfo = "Enum";
            }
            else if (type.IsValueType)
            {
                typeInfo = "Struct";
            }

            if (type.IsAbstract && !type.IsInterface)
            {
                typeInfo = "Abstract " + typeInfo;
            }

            document.Heading(1, "{0} {1}", typeName, typeInfo);
            document.Paragraph("Namespace: {0}", type.Namespace);
        }

        private void DocumentType(Type type, XElement documentation, FsPath outputDir)
        {
            MarkdownDocument document = new MarkdownDocument();

            CreateTitle(document, type);
            document.Paragraph(DocumentSelectors.GetSummary(documentation, type.FullName));

            DocumentPropertes(document, type, documentation);
            DocumentMethods(document, type, documentation);

            var file = outputDir.Combine(type.Name + ".md");
            file.WriteFile(_log, document.ToString());
        }

        private void DocumentPropertes(MarkdownDocument document, Type type, XElement documentation)
        {
            var properties = type.IsInterface ? type.GetProperties() : type.GetProperties(BindingFlags.Public);
            if (properties.Length < 1) return;

            document.Heading(2, "Properties");

            foreach (var property in properties)
            {
                var selector = $"{type.FullName}.{property.Name}";

                document.WriteLine("* `{0} {1}`", property.PropertyType.Name, property.Name);
                document.WriteLine("\tGet: {0}, Set: {0}", property.CanRead, property.CanWrite);
                document.WriteLine("\t{0}", DocumentSelectors.GetSummary(documentation, selector));
                document.WriteLine("");
            }
        }

        private void DocumentMethods(MarkdownDocument document, Type type, XElement documentation)
        {
            var methods = type.IsInterface ? type.GetMethods() : type.GetMethods(BindingFlags.Public);
            if (methods.Length < 1) return;

            document.Heading(2, "Methods");

            foreach (var method in methods)
            {
                var selector = $"{type.FullName}.{method.Name}";
                document.WriteLine("* `{0} {1}`", method.ReturnType.Name, method.Name);
                method.GetParameters();
            }
        }

        public void Document(FsPath assembly, FsPath xmlFile, FsPath outputDir)
        {
            IEnumerable<Type> documentableTypes = GetDocumentableTypes(assembly);
            XElement documentation = XElement.Load(xmlFile.ToString());
            foreach (var type in documentableTypes)
            {
                _log.Info("Documenting type: {0}", type.FullName);
                DocumentType(type, documentation, outputDir);
            }
        }
    }
}
