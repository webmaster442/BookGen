//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Xml.Linq;

namespace BookGen.AssemblyDocumenter.Internals
{
    internal static class PropertyDocumenter
    {
        internal static void DocumentPropertes(MarkdownDocument document, Type type, XElement documentation)
        {
            var properties = type.IsInterface ? type.GetProperties() : type.GetProperties(BindingFlags.Public);
            if (properties.Length < 1) return;

            document.Heading(2, "Properties");

            foreach (var property in properties)
            {
                var selector = $"{type.FullName}.{property.Name}";

                document.WriteLine("* `{0} {1} {2}`", Helpers.GetTypeName(property.PropertyType), property.Name, GetSet(property));
                document.WriteLine("\t{0}", DocumentSelectors.GetPropertyOrTypeSummary(documentation, selector));
                document.WriteLine("");
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
