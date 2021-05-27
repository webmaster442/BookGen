using BookGen.Api;
using BookGen.AssemblyDocument.XmlDoc;
using System;

namespace BookGen.AssemblyDocument
{
    internal abstract class DocumenterBase
    {
        public DocumenterBase(Doc xmlDocumentation,
                              ILog log)
        {
            XmlDocumentation = xmlDocumentation;
            Log = log;
        }

        protected Doc XmlDocumentation { get; }
        protected ILog Log { get; }

        public abstract void Execute(Type type, MarkdownBuilder output);
    }
}
