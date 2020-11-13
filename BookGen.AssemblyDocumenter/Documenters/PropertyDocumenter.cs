//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.AssemblyDocumenter.Internals;
using System;
using System.Reflection;
using System.Xml.Linq;

namespace BookGen.AssemblyDocumenter.Documenters
{
    internal class PropertyDocumenter : DocumenterBase
    {
        public override void Document(MarkdownDocument targetDocument, Type type, XElement docSource)
        {
            var properties = type.IsInterface ? type.GetProperties() : type.GetProperties(BindingFlags.Public);
            if (properties.Length < 1) return;

            targetDocument.Heading(2, "Properties");

            foreach (var property in properties)
            {
                var selector = $"{type.FullName}.{property.Name}";

                targetDocument.WriteLine("* `{0} {1} {2}`", GetTypeName(property.PropertyType), property.Name, GetSet(property));
                targetDocument.WriteLine("    {0}", DocumentSelectors.GetPropertyOrTypeSummary(docSource, selector));
                targetDocument.WriteLine("");
            }
        }

        private static string GetSet(PropertyInfo property)
        {
            var str = "{ ";

            if (property.CanRead)
                str += "get; ";

            if (property.CanWrite)
                str += "set; ";

            return str + " }";
        }
    }
}
