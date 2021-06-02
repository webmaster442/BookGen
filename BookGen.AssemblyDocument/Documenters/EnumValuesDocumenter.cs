using BookGen.Api;
using BookGen.AssemblyDocument.XmlDoc;
using System;
using System.Collections.Generic;

namespace BookGen.AssemblyDocument.Documenters
{
    internal class EnumValuesDocumenter : DocumenterBase
    {
        public EnumValuesDocumenter(Doc xmlDocumentation, ILog log) : base(xmlDocumentation, log)
        {
        }

        public override bool CanExecute(Type type)
        {
            return type.IsEnum;
        }

        public override void Execute(Type type, MarkdownBuilder output)
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            foreach (var name in type.GetEnumNames())
            {
                items.Add(name, XmlDocumentation.GetEnumValueSummary(type, name));
            }
            output.Table(items, "Value", "Summary");
        }
    }
}