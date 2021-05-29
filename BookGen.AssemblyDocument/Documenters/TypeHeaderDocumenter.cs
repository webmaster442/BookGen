using BookGen.Api;
using BookGen.AssemblyDocument.XmlDoc;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGen.AssemblyDocument.Documenters
{
    internal class TypeHeaderDocumenter : DocumenterBase
    {
        public TypeHeaderDocumenter(Doc xmlDocumentation, ILog log) : base(xmlDocumentation, log)
        {
        }

        private string CreateInheritanceChain(IEnumerable<Type> types)
        {
            var builder = new StringBuilder(1024);
            foreach (var type in types)
            {
                builder.Append($"    * {type.GetDocLinkFromType()}\n");
            }
            return builder.ToString();
        }

        private string CreateInterfaceList(Type type)
        {
            var ifaces = type.GetInterfaces();
            if (ifaces.Length < 1)
                return "* No interfaces are implemented";

            var builder = new StringBuilder(1024);
            foreach (var iface in ifaces)
            {
                builder.Append($"    * {iface.GetDocLinkFromType()}\n");
            }
            return builder.ToString();
        }

        public override void Execute(Type type, MarkdownBuilder output)
        {
            var t = type.GetTypeType();

            output.H1($"{type.GetTypeNameForTitle()} {t}");
            if (t == Domain.TypeType.Class 
                || t == Domain.TypeType.Struct 
                || t == Domain.TypeType.Record)
            {
                output.Paragraph("Inheritance chain:");
                output.Paragraph(CreateInheritanceChain(type.GetInheritanceChain()));
            }

            if (t != Domain.TypeType.Delegate &&
                t != Domain.TypeType.Enum)
            {
                output.Paragraph("Implemented interfaces:");
                output.Paragraph(CreateInterfaceList(type));
            }

            output.Paragraph(XmlDocumentation.GetTypeSummary(type));
            var remarks = XmlDocumentation.GetTypeRemarks(type);
            if (!string.IsNullOrEmpty(remarks))
            {
                output.Paragraph("**Remarks:**");
                output.Paragraph(remarks);
            }
        }
    }
}
