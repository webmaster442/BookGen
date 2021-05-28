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
            var bulder = new StringBuilder();
            foreach (var type in types)
            {
                string link = $"type://{type.FullName}";
                if (type.FullName?.StartsWith("System") ?? false)
                {
                    link = $"https://docs.microsoft.com/en-us/dotnet/api/{type.FullName}";
                }
                bulder.Append($"    * [{type.GetNormalizedTypeName()}]({link})\n");
            }
            return bulder.ToString();
        }

        public override void Execute(Type type, MarkdownBuilder output)
        {
            output.H1($"{type.GetTypeNameForTitle()} {type.GetTypeType()}");
            output.Paragraph($"Inheritance chain:");
            output.Paragraph(CreateInheritanceChain(type.GetInheritanceChain()));
        }

    }
}
