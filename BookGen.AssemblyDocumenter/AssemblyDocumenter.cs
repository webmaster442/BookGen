//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.AssemblyDocumenter.Internals;
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

        private void CreatePageTitle(MarkdownDocument document, Type type)
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

            CreatePageTitle(document, type);
            document.Paragraph(DocumentSelectors.GetPropertyOrTypeSummary(documentation, type.FullName));

            if (type.IsEnum)
            {
                EnumDocumenter.DocumentEnum(document, type, documentation);
            }
            else
            {
                PropertyDocumenter.DocumentPropertes(document, type, documentation);
                MethodDocumenter.DocumentMethods(document, type, documentation);
            }

            var file = outputDir.Combine(type.Name + ".md");
            file.WriteFile(_log, document.ToString());
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
