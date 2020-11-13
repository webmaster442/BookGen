//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.AssemblyDocumenter.Internals;
using System;
using System.Xml.Linq;

namespace BookGen.AssemblyDocumenter.Documenters
{
    internal class EnumDocumenter : DocumenterBase
    {
        public override void Document(MarkdownDocument targetDocument, Type type, XElement docSource)
        {
            targetDocument.Heading(2, "Items");

            foreach (var item in type.GetEnumNames())
            {
                var selector = $"{type.FullName}.{item}";

                targetDocument.WriteLine("* `{0}`", item);
                targetDocument.WriteLine("    {0}", DocumentSelectors.GetPropertyOrTypeSummary(docSource, selector));
                targetDocument.WriteLine("");
            }
        }
    }
}
