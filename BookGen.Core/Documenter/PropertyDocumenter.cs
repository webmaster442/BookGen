//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Xml.Linq;
using System.Linq;

namespace BookGen.Core.Documenter
{
    internal class PropertyDocumenter : DocumenterBase
    {
        public override void Document(MarkdownWriter targetDocument, Type type, XElement docSource)
        {
            var properties = type.IsInterface ? type.GetProperties() : type.GetProperties(BindingFlags.Public);
            if (properties.Length < 1) return;

            targetDocument.Heading(2, "Properties");

            foreach (var property in properties)
            {
                var selector = $"{type.FullName}.{property.Name}";

                targetDocument.WriteLine("* `{0} {1} {2}`", GetTypeName(property.PropertyType), property.Name, GetInfo(property));
                targetDocument.WriteLine("    {0}", Selectors.GetPropertyOrTypeSummary(docSource, selector));
                targetDocument.WriteLine("");
            }
        }

        private static string GetInfo(PropertyInfo property)
        {
            var str = "{ ";

            if (property.CanRead)
                str += "get; ";

            Type[] modifiers = property.SetMethod?.ReturnParameter.GetRequiredCustomModifiers() ?? Array.Empty<Type>();

            if (modifiers.Contains(typeof(System.Runtime.CompilerServices.IsExternalInit)))
            {
                str += "init; ";
            }
            else
            {
                str += "set; ";
            }

            return str + " }";
        }
    }
}
