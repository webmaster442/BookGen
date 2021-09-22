//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Xml.Linq;

namespace BookGen.Core.Documenter
{
    internal class EnumDocumenter : DocumenterBase
    {
        public override void Document(MarkdownWriter targetDocument, Type type, XElement docSource)
        {
            targetDocument.Heading(2, "Items");

            foreach (var item in type.GetEnumNames())
            {
                var selector = $"{type.FullName}.{item}";

                targetDocument.WriteLine("* `{0}`", item);
                targetDocument.WriteLine("    {0}", Selectors.GetPropertyOrTypeSummary(docSource, selector));
                targetDocument.WriteLine("");
            }
        }
    }
}
