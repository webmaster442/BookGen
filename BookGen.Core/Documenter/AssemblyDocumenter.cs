//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core.Documenter.Documenters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace BookGen.Core.Documenter
{
    public class AssemblyDocumenter
    {
        private readonly ILog _log;
        private readonly EnumDocumenter _enumDocumenter;
        private readonly MethodDocumenter _methodDocumenter;
        private readonly PropertyDocumenter _propertyDocumenter;
        public AssemblyDocumenter(ILog log)
        {
            _log = log;
            _enumDocumenter = new EnumDocumenter();
            _methodDocumenter = new MethodDocumenter();
            _propertyDocumenter = new PropertyDocumenter();
        }



        private static void CreatePageTitle(MarkdownDocument document, Type type)
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
            document.Paragraph("Namespace: {0}", type?.Namespace ?? string.Empty);
        }

        private void DocumentType(Type type, XElement documentation, FsPath outputDir)
        {
            MarkdownDocument document = new MarkdownDocument();

            CreatePageTitle(document, type);
            document.Paragraph(DocumentSelectors.GetPropertyOrTypeSummary(documentation, type?.FullName ?? string.Empty));

            if (type?.IsEnum == true)
            {
                _enumDocumenter.Document(document, type, documentation);
            }
            else
            {
                _propertyDocumenter.Document(document, type!, documentation);
                _methodDocumenter.Document(document, type!, documentation);
            }

            var file = outputDir.Combine(type?.Name + ".md");
            file.WriteFile(_log, document.ToString());
        }

        public void Document(FsPath assembly, FsPath xmlFile, FsPath outputDir)
        {
            /*IEnumerable<Type> documentableTypes = GetDocumentableTypes(assembly);
            XElement documentation = XElement.Load(xmlFile.ToString());
            foreach (var type in documentableTypes)
            {
                if (type != null)
                {
                    _log.Info("Documenting type: {0}", type?.FullName ?? string.Empty);
                    DocumentType(type!, documentation, outputDir);
                }
            }*/
        }
    }
}
