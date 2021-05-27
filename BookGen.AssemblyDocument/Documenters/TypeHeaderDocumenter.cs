using BookGen.Api;
using BookGen.AssemblyDocument.XmlDoc;
using System;

namespace BookGen.AssemblyDocument.Documenters
{
    internal class TypeHeaderDocumenter : DocumenterBase
    {
        public TypeHeaderDocumenter(Doc xmlDocumentation, ILog log) : base(xmlDocumentation, log)
        {
        }

        public override void Execute(Type type, MarkdownBuilder output)
        {
            throw new NotImplementedException();
        }
    }
}
