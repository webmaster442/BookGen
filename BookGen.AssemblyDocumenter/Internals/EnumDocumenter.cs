//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Xml.Linq;


namespace BookGen.AssemblyDocumenter.Internals
{
    internal static class EnumDocumenter
    {
        internal static void DocumentEnum(MarkdownDocument document, Type type, XElement documentation)
        {
            document.Heading(2, "Items");

            foreach (var item in type.GetEnumNames())
            {
                var selector = $"{type.FullName}.{item}";

                document.WriteLine("* `{0}`", item);
                document.WriteLine("    {0}", DocumentSelectors.GetPropertyOrTypeSummary(documentation, selector));
                document.WriteLine("");
            }
        }
    }
}
