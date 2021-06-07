using BookGen.AssemblyDocument.XmlDoc;
using System;
using System.Linq;
using System.Reflection;

namespace BookGen.AssemblyDocument
{
    public static class Selectors
    {
        private static string GetTypeSelectorName(this Type type)
        {
            return $"T:{type.FullName}";
        }

        private static string GetPropertySelectorName(this PropertyInfo property)
        {
            return $"P:{property.ReflectedType?.FullName}.{property.Name}";
        }

        private static string GetFieldSelectorName(this Type type, string field)
        {
            return $"F:{type.FullName}.{field}";
        }

        private static string GetMethodSelectorName(this MethodInfo method)
        {
            return $"M:{method.ReflectedType?.FullName}.{method.Name}";
        }

        private static string GetElementText<T>(Member? member) where T : Content
        {
            if (member != null)
            {
                return member.Items?.OfType<T>()?.FirstOrDefault()?.NormalizedText ?? string.Empty;
            }
            return string.Empty;
        }

        public static string GetTypeSummary(this Doc documentation, Type t)
        {
            var member = Array.Find(documentation.Members.Items, m => m.Name == t.GetTypeSelectorName());
            return GetElementText<Summary>(member);
        }

        public static string GetTypeRemarks(this Doc documentation, Type t)
        {
            var member = Array.Find(documentation.Members.Items, m => m.Name == t.GetTypeSelectorName());
            return GetElementText<Remarks>(member);
        }

        public static string GetPropertySummary(this Doc documentation, PropertyInfo property)
        {
            var member = Array.Find(documentation.Members.Items, m => m.Name == property.GetPropertySelectorName());
            return GetElementText<Summary>(member);
        }

        public static string GetMethodSummary(this Doc documentation, MethodInfo methodInfo)
        {
            var member = Array.Find(documentation.Members.Items, m => m.Name == methodInfo.GetMethodSelectorName());
            return GetElementText<Summary>(member);
        }

        public static string GetEnumValueSummary(this Doc documentation, Type type, string enumItem)
        {
            var member = Array.Find(documentation.Members.Items, m => m.Name == type.GetFieldSelectorName(enumItem));
            if (member != null)
            {
                return member.Items?.OfType<Summary>()?.FirstOrDefault()?.NormalizedText ?? string.Empty;
            }
            return string.Empty;
        }
    }
}
