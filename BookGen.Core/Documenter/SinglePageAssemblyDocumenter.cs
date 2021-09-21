//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace BookGen.Core.Documenter
{
    public sealed class SinglePageAssemblyDocumenter
    {
        private readonly ILog _log;
        private readonly EnumDocumenter _enumDocumenter;
        private readonly MethodDocumenter _methodDocumenter;
        private readonly PropertyDocumenter _propertyDocumenter;

        public SinglePageAssemblyDocumenter(ILog log)
        {
            _log = log;
            _enumDocumenter = new EnumDocumenter();
            _methodDocumenter = new MethodDocumenter();
            _propertyDocumenter = new PropertyDocumenter();
        }
        public void Document(FsPath assemblyPath, FsPath outputPath)
        {
            if (!assemblyPath.IsFile)
            {
                _log.Critical("Can't find assembly file: {0}", assemblyPath);
                return;
            }

            MarkdownWriter mardkown = new MarkdownWriter();

            var xml = Path.ChangeExtension(assemblyPath.ToString(), "xml");
            if (!File.Exists(xml))
            {
                _log.Critical("Can't find xml file: {0}", xml);
                return;
            }

            IEnumerable<Type> documentableTypes = GetDocumentableTypes(assemblyPath);
            XElement documentation = XElement.Load(xml);

            foreach (var type in documentableTypes)
            {
                if (type != null)
                {
                    _log.Detail("Documenting: {0}", type.FullName ?? string.Empty);
                    DocumentType(type, documentation, mardkown);
                    mardkown.HorizontalLine();
                }
            }

            outputPath.WriteFile(_log, mardkown.ToString());
        }

        private void CreatePageTitle(MarkdownWriter document, Type type)
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

        private IEnumerable<Type> GetDocumentableTypes(FsPath assemblyPath)
        {
            var asm = Assembly.LoadFrom(assemblyPath.ToString());
            return asm.GetExportedTypes();
        }

        private void DocumentType(Type type, XElement documentation, MarkdownWriter mardkown)
        {
            CreatePageTitle(mardkown, type);
            mardkown.Paragraph(Selectors.GetPropertyOrTypeSummary(documentation, type.FullName ?? string.Empty));

            if (type.IsEnum)
            {
                _enumDocumenter.Document(mardkown, type, documentation);
            }
            else
            {
                //complex type
                _propertyDocumenter.Document(mardkown, type, documentation);
                _methodDocumenter.Document(mardkown, type, documentation);
            }
        }

    }
}
