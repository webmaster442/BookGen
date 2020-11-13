// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.AssemblyDocumenter.Internals;
using System;
using System.Linq;
using System.Xml.Linq;

namespace BookGen.AssemblyDocumenter.Documenters
{
    internal abstract class DocumenterBase
    {
        public abstract void Document(MarkdownDocument targetDocument, Type type, XElement docSource);

        protected static string GetTypeName(Type type)
        {
            if (!type.IsGenericType)
                return type.Name;

            var nameandArgs = type.Name.Split('`');
            var genericPars = string.Join(", ", type.GetGenericArguments().Select(x => x.Name));

            return $"{nameandArgs[0]}<{genericPars}>";
        }
    }
}
